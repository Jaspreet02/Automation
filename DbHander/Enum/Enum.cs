using System.ComponentModel;

namespace DbHander
{
    public enum EmailStatusType
    {
        Ready = 0,
        Sending = 1,
        Success = 2,
        Error = 3
    }
    public enum QueueTypes
    {
        SFTP = 1,
        FTP = 2,
        SharedPath = 3,
        LocalFileSystem = 4
    }
    public enum JobStatusType
    {
        Running = 1,
        Error = 2,
        Complete = 3,
        Status_Killed = 6,
        Status_TimeExp = 7
    }
    public enum SessionStatusType
    {
        Status_Error = 2,
        Status_Complete = 3
    }
    public enum ComponentStatusType
    {
        Ready = 0,
        Running = 1,
        Error = 2,
        Completed = 3,
        Optional = 4
    }
    public enum RunNumberStatusType
    {
        Ready = 0,
        Running = 1,
        Error = 2,
        Completed = 3
    }
    public enum ValidationType
    {
        Default = 1,
        Batch = 2,
        Custom = 0
    }

    public enum EmailKeyword
    {
        [Description("Send email for forget password")]
        FORGET_PASSWORD = 1,
        [Description("New user email")]
        WELCOME_EMAIL = 2,
        [Description("Notification for file download")]
        DOWNLOAD_FILE = 3,
        [Description("Notification for component error")]
        COMPONENT_ERROR = 4,
        [Description("Notification for component success")]
        COMPONENT_SUCCESS = 5,
        [Description("Email notification")]
        NOTIFICATION_EMAIL = 6
    }

    public enum FileKeyword
    {
        RUN_NUMBER,
        CLIENT_NAME,
        APP_NAME,
        FILENAME_EXT,
        FILE_NAME,
        FILENAME_WITHOUT_EXT,
        NOW_dd_MM_yyyy,
        NOW_dd_MM_yy,
        NOW_dd_MM,
        NOW_MM_dd_yyyy,
        NOW_MM_dd_yy,
        NOW_MM_dd,
        NOW_yyyy_MM_dd,
        NOW_yy_MM_dd,
        NOW_yy_MM,
    }

}
