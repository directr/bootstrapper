using System.IO;

namespace BootStrapper
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using Args;
    using Args.Help;
    using Autofac;
    using AutoMapper;
    using Cumulux.BootStrapper;

    internal class Program
    {
        private static IArgumentsParser parser = new AzureArgumentsParser();

        internal static void Main(string[] args)
        {
            try
            {
                // Expand fist and then bind to Args
                var expanded = args.Select(a => parser.ParseArguments(a)).ToArray();

                var config = Args.Configuration.Configure<CommandArgs>();

                var cmds = config.AsFluent()
                                 .UsingStringComparer(StringComparer.InvariantCultureIgnoreCase)
                                 .Initialize()
                                 .CreateAndBind(expanded);

                if (args.Length < 1 || args.Length == 1 && cmds.ShowHelp)
                {
                    var help = new HelpProvider();
                    var commandsHelp = help.GenerateModelHelp<CommandArgs>(config);

                    var formatter = new Args.Help.Formatters.ConsoleHelpFormatter();
                    formatter.WriteHelp(commandsHelp, Console.Out);

                    return;
                }

                // TODO: Research for a better way to validate parameters with Args
                try
                {
                    ValidateArgs(cmds);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine();
                    Console.WriteLine("Try bootstrapper -help for more information");
                }

                Mapper.CreateMap<CommandArgs, BootStrapperArgs>()
                          .ForMember(dest => dest.Unzip, opt => opt.MapFrom(src => !String.IsNullOrEmpty(src.Unzip)))
                          .ForMember(dest => dest.UnzipTarget, opt => opt.MapFrom(src => parser.ParseArguments(src.Unzip)));

                var bootArgs = Mapper.Map<CommandArgs, BootStrapperArgs>(cmds);

                var builder = BuildContainer();

                using (var container = builder.Build())
                {
                    container.Resolve<BootStrapperManager>().Start(bootArgs);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

                // TODO: Push error to blob storage?
            }
        }

        private static void ValidateArgs(CommandArgs command)
        {
            if ( ( !String.IsNullOrEmpty(command.Get) && String.IsNullOrEmpty(command.Unzip)) ||
                 !String.IsNullOrEmpty(command.Put) )
            {
                if ( String.IsNullOrEmpty(command.LocalResource) )
                    throw new ArgumentException("LocalResource is required.");
                try
                {
                    new FileInfo(command.LocalResource);
                }
                catch ( ArgumentException ae )
                {
                    throw new ArgumentException("Invalid local resource path.", ae);
                }
                catch ( PathTooLongException pl )
                {
                    throw new PathTooLongException("Local resource exceeds allowable path length.", pl);
                }
                catch ( NotSupportedException ns )
                {
                    throw new ArgumentException("The local resource path not supported.", ns);
                }
            }

            if ( String.IsNullOrEmpty(command.StorageConnection) )
            {
                if ( !String.IsNullOrEmpty(command.Get) && !Uri.IsWellFormedUriString(command.Get, UriKind.Absolute) )
                    throw new ArgumentException("A valid connection string to Azure blob storage must be supplied when downloading from a relative URL.", command.Get);
                if ( !String.IsNullOrEmpty(command.Put) && !Uri.IsWellFormedUriString(command.Put, UriKind.Absolute) )
                    throw new ArgumentException("A valid connection string to Azure blob storage must be supplied when uploading to a relative URL.", command.Get);
            }
        }

        private static ContainerBuilder BuildContainer()
        {
            var builder = new ContainerBuilder();
            builder.Register(c => new DebugLogger()).As<ILogger>();

            builder.Register(c => new BootStrapperManager(
                c.Resolve<ILogger>(),
                c.Resolve<IPackageDownloader>(),
                c.Resolve<IPackageRunner>(),
                c.Resolve<IPackageUnzipper>(),
                c.Resolve<IPackageUploader>(),
                c.Resolve<IPackageZipper>()));

            builder.Register(c => new PackageDownloader(c.Resolve<ILogger>())).As<IPackageDownloader>();
            builder.Register(c => new PackageRunner(c.Resolve<ILogger>())).As<IPackageRunner>();
            builder.Register(c => new PackageUnzipper(c.Resolve<ILogger>())).As<IPackageUnzipper>();
            builder.Register(c => new PackageUploader(c.Resolve<ILogger>())).As<IPackageUploader>();
            builder.Register(c => new PackageZipper(c.Resolve<ILogger>())).As<IPackageZipper>();
            builder.Register(c => new TraceLogger()).As<ILogger>();

            return builder;
        }
    }
}
