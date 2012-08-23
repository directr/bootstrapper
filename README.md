bootstrapper
============

BootStrapper Help v1.1
<command> [-Get] [-storage] [-Put] [-unzip] [-lr] [-Run] [-Args] [-runAlways] 
          [-overwrite] [-Block] [-enable] [-runInEmulator] [-help]


[-Get|-G]                The full address (http://) of the file to download or 
                         relative URI when downloading from Windows Azure Blob 
                         Storage
[-storage|-sc]           Windows Azure Storage Connection String
[-Put|-P]                Fully qualified target URL (http://) of the upload 
                         file location or relative URI when
uploading to 
                         Windows Azure Blob Storage.
[-unzip]                 The destination folder for the contents of the zip 
                         file
[-lr|-localResource]     The local resource path.
[-Run]                   Absolute path to the executable to start.  By 
                         default, this will only run once - even on reboots
[-Args|-A]               Arguments to pass to the executable invoked with -run
[-runAlways]             Force to bootstrapper to always download, always 
                         unzip, and always execute the -run option
[-overwrite]             Force to bootstrapper to overwrite any existing files 
                         when download, uploading, or unpacking.
[-Block|-B]              Blocks the Run command until exit.  Not a good idea 
                         for starting a daemon/service
[-enable|-esp]           Enable system profile.  This is a special mode for 
                         some installers that require a user profile location.
[-runInEmulator]         Run Bootstrapper when running under Azure Emulated 
                         environment
[-help|-h]               Shows the program help

