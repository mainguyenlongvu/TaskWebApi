namespace TaskWebApi.Enum
{
    public enum ApplicationEnum
    {
    }

    public enum Sex
    {
        Male = 1,
        Female = 2
    }
    public enum WagetType
    {
        basic,
        medium,
        advanced
    }

    public enum Roles
    {
        memmber,
        Admin,
    }
    public enum ApplicationType
    {
        Leave_Application,
        Report_Application,
        Resignation_Application
    }

    public enum ApplicationStatus
    {
        Proccessing,
        Approved,
        Rejected
    }
}
