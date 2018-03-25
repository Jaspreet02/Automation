namespace DbHander.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _06082017 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AppCommandArguments",
                c => new
                    {
                        AppCommandArgumentId = c.Int(nullable: false, identity: true),
                        AppSchedulerId = c.Int(nullable: false),
                        Order = c.Int(nullable: false),
                        Argument = c.String(),
                        Status = c.Boolean(nullable: false),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        ModifiedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.AppCommandArgumentId);
            
            CreateTable(
                "dbo.ApplicationComponents",
                c => new
                    {
                        ApplicationComponentId = c.Int(nullable: false, identity: true),
                        ApplicationId = c.Int(nullable: false),
                        ComponentId = c.Int(nullable: false),
                        ComponentOrder = c.Int(nullable: false),
                        IsOptional = c.Boolean(nullable: false),
                        Status = c.Boolean(nullable: false),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        ModifiedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.ApplicationComponentId)
                .ForeignKey("dbo.Applications", t => t.ApplicationId, cascadeDelete: true)
                .ForeignKey("dbo.Components", t => t.ComponentId, cascadeDelete: true)
                .Index(t => t.ApplicationId)
                .Index(t => t.ComponentId);
            
            CreateTable(
                "dbo.Applications",
                c => new
                    {
                        ApplicationId = c.Int(nullable: false, identity: true),
                        ClientId = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 30),
                        Code = c.String(nullable: false, maxLength: 3),
                        ProcessingType = c.Int(nullable: false),
                        IsBatch = c.Boolean(nullable: false),
                        SLA = c.Int(nullable: false),
                        FileTransferSettingId = c.Int(nullable: false),
                        HotFolder = c.String(nullable: false),
                        ArchivePath = c.String(),
                        ArchiveFileName = c.String(),
                        IsArchive = c.Boolean(nullable: false),
                        IsFileMove = c.Boolean(nullable: false),
                        InputPath = c.String(nullable: false),
                        ArchivalDays = c.Int(nullable: false),
                        Status = c.Boolean(nullable: false),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        ModifiedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.ApplicationId)
                .ForeignKey("dbo.Clients", t => t.ClientId, cascadeDelete: true)
                .ForeignKey("dbo.FileTransferSettings", t => t.FileTransferSettingId, cascadeDelete: true)
                .Index(t => t.ClientId)
                .Index(t => t.FileTransferSettingId);
            
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        ClientId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 30),
                        Code = c.String(nullable: false, maxLength: 3),
                        EmailAddress = c.String(),
                        Contact = c.String(nullable: false, maxLength: 10),
                        ProofFormat = c.String(maxLength: 3),
                        ProofPassword = c.String(),
                        ProofName = c.String(),
                        UserId = c.Int(nullable: false),
                        ProofsAge = c.Int(nullable: false),
                        Status = c.Boolean(nullable: false),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        ModifiedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.ClientId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 20),
                        MiddleName = c.String(maxLength: 20),
                        LastName = c.String(nullable: false, maxLength: 20),
                        Password = c.String(nullable: false),
                        EmailAddress = c.String(nullable: false),
                        ContactNumber = c.String(nullable: false, maxLength: 10),
                        Gender = c.String(nullable: false),
                        IsLocked = c.Boolean(nullable: false),
                        ResetToken = c.String(),
                        TokenExpired = c.Boolean(nullable: false),
                        ParentId = c.Int(nullable: false),
                        Salt = c.Int(nullable: false),
                        Status = c.Boolean(nullable: false),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        ModifiedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.FileTransferSettings",
                c => new
                    {
                        FileTransferSettingId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        QueueTypeId = c.Byte(nullable: false),
                        Host = c.String(),
                        UserName = c.String(),
                        Password = c.String(),
                        Port = c.String(),
                        UserId = c.Int(nullable: false),
                        Status = c.Boolean(nullable: false),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        ModifiedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.FileTransferSettingId)
                .ForeignKey("dbo.QueueTypes", t => t.QueueTypeId, cascadeDelete: true)
                .Index(t => t.QueueTypeId);
            
            CreateTable(
                "dbo.QueueTypes",
                c => new
                    {
                        QueueTypeId = c.Byte(nullable: false),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.QueueTypeId);
            
            CreateTable(
                "dbo.Components",
                c => new
                    {
                        ComponentId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 30),
                        ComponentExe = c.String(nullable: false),
                        Detail = c.String(),
                        ShortName = c.String(nullable: false, maxLength: 3),
                        IsOptional = c.Boolean(nullable: false),
                        Status = c.Boolean(nullable: false),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        ModifiedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.ComponentId);
            
            CreateTable(
                "dbo.TriggerandStatusFiles",
                c => new
                    {
                        ComponentId = c.Int(nullable: false),
                        TriggerFilelocation = c.String(),
                        StepStatusLocation = c.String(),
                        Status = c.Boolean(nullable: false),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        ModifiedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.ComponentId)
                .ForeignKey("dbo.Components", t => t.ComponentId)
                .Index(t => t.ComponentId);
            
            CreateTable(
                "dbo.ApplicationConfigFiles",
                c => new
                    {
                        ApplicationConfigFileId = c.Int(nullable: false, identity: true),
                        ApplicationId = c.Int(nullable: false),
                        PDFPage = c.Int(nullable: false),
                        Status = c.Boolean(nullable: false),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        ModifiedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.ApplicationConfigFileId)
                .ForeignKey("dbo.Applications", t => t.ApplicationId, cascadeDelete: true)
                .Index(t => t.ApplicationId);
            
            CreateTable(
                "dbo.ApplicationFiles",
                c => new
                    {
                        ApplicationFileId = c.Int(nullable: false, identity: true),
                        ApplicationId = c.Int(nullable: false),
                        Mask = c.String(nullable: false),
                        IsRequired = c.Boolean(nullable: false),
                        ProofRestrictedDays = c.Int(nullable: false),
                        Status = c.Boolean(nullable: false),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        ModifiedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.ApplicationFileId)
                .ForeignKey("dbo.Applications", t => t.ApplicationId, cascadeDelete: true)
                .Index(t => t.ApplicationId);
            
            CreateTable(
                "dbo.ApplicationSmtps",
                c => new
                    {
                        ApplicationSmtpId = c.Int(nullable: false, identity: true),
                        ApplicationId = c.Int(nullable: false),
                        SmtpId = c.Int(nullable: false),
                        Status = c.Boolean(nullable: false),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        ModifiedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.ApplicationSmtpId)
                .ForeignKey("dbo.Applications", t => t.ApplicationId, cascadeDelete: true)
                .Index(t => t.ApplicationId);
            
            CreateTable(
                "dbo.ClientSmtps",
                c => new
                    {
                        ClientSmtpId = c.Int(nullable: false, identity: true),
                        ClientId = c.Int(nullable: false),
                        SmtpId = c.Int(nullable: false),
                        Status = c.Boolean(nullable: false),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        ModifiedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.ClientSmtpId);
            
            CreateTable(
                "dbo.ComponentConfigs",
                c => new
                    {
                        ComponentConfigId = c.Int(nullable: false, identity: true),
                        ApplicationComponentId = c.Int(nullable: false),
                        ConfigFile = c.String(),
                        ConfigName = c.String(),
                        Status = c.Boolean(nullable: false),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        ModifiedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.ComponentConfigId)
                .ForeignKey("dbo.ApplicationComponents", t => t.ApplicationComponentId, cascadeDelete: true)
                .Index(t => t.ApplicationComponentId);
            
            CreateTable(
                "dbo.ComponentInputLocations",
                c => new
                    {
                        ComponentInputLocationId = c.Int(nullable: false, identity: true),
                        ComponentId = c.Int(nullable: false),
                        ApplicationId = c.Int(nullable: false),
                        TagName = c.String(nullable: false),
                        FileMask = c.String(nullable: false),
                        InputLocation = c.String(nullable: false),
                        Status = c.Boolean(nullable: false),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        ModifiedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.ComponentInputLocationId)
                .ForeignKey("dbo.Applications", t => t.ApplicationId, cascadeDelete: true)
                .ForeignKey("dbo.Components", t => t.ComponentId, cascadeDelete: true)
                .Index(t => t.ComponentId)
                .Index(t => t.ApplicationId);
            
            CreateTable(
                "dbo.ComponentOutputLocations",
                c => new
                    {
                        ComponentOutputLocationId = c.Int(nullable: false, identity: true),
                        ComponentId = c.Int(nullable: false),
                        ApplicationId = c.Int(nullable: false),
                        TagName = c.String(nullable: false),
                        FileMask = c.String(nullable: false),
                        OutputLocation = c.String(nullable: false),
                        Status = c.Boolean(nullable: false),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        ModifiedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.ComponentOutputLocationId)
                .ForeignKey("dbo.Applications", t => t.ApplicationId, cascadeDelete: true)
                .ForeignKey("dbo.Components", t => t.ComponentId, cascadeDelete: true)
                .Index(t => t.ComponentId)
                .Index(t => t.ApplicationId);
            
            CreateTable(
                "dbo.ComponentStatus",
                c => new
                    {
                        ComponentStatusId = c.Byte(nullable: false),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.ComponentStatusId);
            
            CreateTable(
                "dbo.ContactInfoes",
                c => new
                    {
                        ContactInfoId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ContactNo = c.String(),
                        EmailAddress = c.String(),
                        Status = c.Boolean(nullable: false),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        ModifiedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.ContactInfoId);
            
            CreateTable(
                "dbo.DeliveryEmailSettings",
                c => new
                    {
                        DeliveryEmailSettingId = c.Int(nullable: false, identity: true),
                        ApplicationId = c.Int(nullable: false),
                        MaskName = c.String(),
                        Mask = c.String(),
                        InputPath = c.String(),
                        IsAttachment = c.Boolean(nullable: false),
                        Status = c.Boolean(nullable: false),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        ModifiedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.DeliveryEmailSettingId)
                .ForeignKey("dbo.Applications", t => t.ApplicationId, cascadeDelete: true)
                .Index(t => t.ApplicationId);
            
            CreateTable(
                "dbo.EmailStatus",
                c => new
                    {
                        EmailStatusId = c.Byte(nullable: false),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.EmailStatusId);
            
            CreateTable(
                "dbo.EmailTemplates",
                c => new
                    {
                        EmailTemplateId = c.Int(nullable: false, identity: true),
                        EmailFromSmtpId = c.Int(nullable: false),
                        EmailToIds = c.String(nullable: false),
                        Subject = c.String(nullable: false, maxLength: 50),
                        EmailLevelId = c.Int(nullable: false),
                        EmailCcIds = c.String(),
                        Body = c.String(nullable: false),
                        EmailToken = c.String(nullable: false),
                        ClientId = c.Int(nullable: false),
                        ApplicationId = c.Int(nullable: false),
                        ApplicationComponentId = c.Int(nullable: false),
                        TimeInterval = c.Int(nullable: false),
                        Status = c.Boolean(nullable: false),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        ModifiedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.EmailTemplateId);
            
            CreateTable(
                "dbo.EmailTokens",
                c => new
                    {
                        EmailTokenId = c.Byte(nullable: false),
                        Note = c.String(),
                        Keyword = c.String(),
                    })
                .PrimaryKey(t => t.EmailTokenId);
            
            CreateTable(
                "dbo.EmailTrackings",
                c => new
                    {
                        EmailTrackingId = c.Int(nullable: false, identity: true),
                        EmailTemplateId = c.Int(nullable: false),
                        RunNumberId = c.Int(nullable: false),
                        FromEmailId = c.String(),
                        EmailToIds = c.String(),
                        EmailCcIds = c.String(),
                        Subjects = c.String(),
                        Body = c.String(),
                        SentMessage = c.String(),
                        EmailStatus = c.Int(nullable: false),
                        SentDate = c.DateTime(nullable: false),
                        Status = c.Boolean(nullable: false),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        ModifiedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.EmailTrackingId)
                .ForeignKey("dbo.EmailTemplates", t => t.EmailTemplateId, cascadeDelete: true)
                .ForeignKey("dbo.RunDetails", t => t.RunNumberId, cascadeDelete: true)
                .Index(t => t.EmailTemplateId)
                .Index(t => t.RunNumberId);
            
            CreateTable(
                "dbo.RunDetails",
                c => new
                    {
                        RunDetailId = c.Int(nullable: false, identity: true),
                        RunNumber = c.String(nullable: false),
                        ApplicationId = c.Int(nullable: false),
                        RunNumberStatusId = c.Byte(nullable: false),
                        Status = c.Boolean(nullable: false),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        ModifiedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.RunDetailId)
                .ForeignKey("dbo.Applications", t => t.ApplicationId, cascadeDelete: true)
                .ForeignKey("dbo.RunNumberStatus", t => t.RunNumberStatusId, cascadeDelete: true)
                .Index(t => t.ApplicationId)
                .Index(t => t.RunNumberStatusId);
            
            CreateTable(
                "dbo.RunNumberStatus",
                c => new
                    {
                        RunNumberStatusId = c.Byte(nullable: false),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.RunNumberStatusId);
            
            CreateTable(
                "dbo.ErrorLogs",
                c => new
                    {
                        ErrorLogId = c.Int(nullable: false, identity: true),
                        ComponentName = c.String(),
                        ErrorTime = c.DateTime(nullable: false),
                        ErrorMessage = c.String(),
                        Exception = c.String(),
                        InnerException = c.String(),
                        StrackTrace = c.String(),
                        AppId = c.Int(nullable: false),
                        CompId = c.Int(nullable: false),
                        RunId = c.Int(nullable: false),
                        AppCompId = c.Int(nullable: false),
                        Status = c.Boolean(nullable: false),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        ModifiedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.ErrorLogId);
            
            CreateTable(
                "dbo.GMCCommands",
                c => new
                    {
                        GMCCommandId = c.Int(nullable: false, identity: true),
                        ComponentId = c.Int(nullable: false),
                        ApplicationId = c.Int(nullable: false),
                        CommandOrder = c.Int(nullable: false),
                        CommandKey = c.String(),
                        CommandValue = c.String(),
                        GmcAddonSchenerioId = c.Int(nullable: false),
                        Status = c.Boolean(nullable: false),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        ModifiedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.GMCCommandId);
            
            CreateTable(
                "dbo.JobStatus",
                c => new
                    {
                        JobStatusId = c.Byte(nullable: false),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.JobStatusId);
            
            CreateTable(
                "dbo.ProcComponents",
                c => new
                    {
                        ProcComponentId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ExecutablePath = c.String(),
                        LicenceLimit = c.Int(nullable: false),
                        Status = c.Boolean(nullable: false),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        ModifiedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.ProcComponentId);
            
            CreateTable(
                "dbo.ProcSessions",
                c => new
                    {
                        ProcSessionId = c.Int(nullable: false, identity: true),
                        ApplicationId = c.Int(nullable: false),
                        ProcessID = c.String(),
                        ProcComponentID = c.Int(nullable: false),
                        SessionKey = c.String(),
                        ProcStatus = c.Byte(nullable: false),
                        KillRequired = c.Boolean(nullable: false),
                        ExpectedDateTime = c.DateTime(),
                        Status = c.Boolean(nullable: false),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        ModifiedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.ProcSessionId)
                .ForeignKey("dbo.Applications", t => t.ApplicationId, cascadeDelete: true)
                .ForeignKey("dbo.JobStatus", t => t.ProcStatus, cascadeDelete: true)
                .Index(t => t.ApplicationId)
                .Index(t => t.ProcStatus);
            
            CreateTable(
                "dbo.ProofFiles",
                c => new
                    {
                        ProofFileId = c.Int(nullable: false, identity: true),
                        ProofId = c.Int(nullable: false),
                        FileName = c.String(),
                        UploadStatus = c.Int(nullable: false),
                        Status = c.Boolean(nullable: false),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        ModifiedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.ProofFileId);
            
            CreateTable(
                "dbo.Proofs",
                c => new
                    {
                        ProofId = c.Int(nullable: false, identity: true),
                        ClientId = c.Int(nullable: false),
                        ApplicationId = c.Int(nullable: false),
                        RunNumberId = c.Int(nullable: false),
                        InputFileName = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        SendDateTime = c.DateTime(),
                        ApproveRejectDate = c.DateTime(),
                        ApproveRejectBy = c.Int(),
                        Notes = c.String(),
                        Status = c.Boolean(nullable: false),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        ModifiedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.ProofId);
            
            CreateTable(
                "dbo.RawFiles",
                c => new
                    {
                        RawFileId = c.Int(nullable: false, identity: true),
                        RunNumberId = c.Int(nullable: false),
                        FileName = c.String(),
                        HotFolder = c.String(),
                        Status = c.Boolean(nullable: false),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        ModifiedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.RawFileId)
                .ForeignKey("dbo.RunDetails", t => t.RunNumberId, cascadeDelete: true)
                .Index(t => t.RunNumberId);
            
            CreateTable(
                "dbo.RunArchiveDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RunNumber = c.String(),
                        OutputFileArchiveId = c.Int(nullable: false),
                        InputFile = c.String(),
                        ArchiveFileName = c.String(),
                        MoveFileName = c.String(),
                        ArchiveTime = c.DateTime(nullable: false),
                        MoveTime = c.DateTime(nullable: false),
                        Status = c.Boolean(nullable: false),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        ModifiedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RunComponentStatus",
                c => new
                    {
                        RunComponentStatusId = c.Int(nullable: false, identity: true),
                        RunNumberId = c.Int(nullable: false),
                        ComponentId = c.Int(nullable: false),
                        ComponentOrder = c.Int(nullable: false),
                        ComponentStatusId = c.Byte(nullable: false),
                        EndDate = c.DateTime(),
                        StartDate = c.DateTime(),
                        Message = c.String(),
                        Status = c.Boolean(nullable: false),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        ModifiedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.RunComponentStatusId)
                .ForeignKey("dbo.Components", t => t.ComponentId, cascadeDelete: true)
                .ForeignKey("dbo.ComponentStatus", t => t.ComponentStatusId, cascadeDelete: true)
                .ForeignKey("dbo.RunDetails", t => t.RunNumberId, cascadeDelete: true)
                .Index(t => t.RunNumberId)
                .Index(t => t.ComponentId)
                .Index(t => t.ComponentStatusId);
            
            CreateTable(
                "dbo.ScheduledFrequencies",
                c => new
                    {
                        ScheduledFrequencyId = c.Int(nullable: false, identity: true),
                        ApplicationId = c.Int(nullable: false),
                        StartTime = c.Time(nullable: false, precision: 7),
                        EndTime = c.Time(nullable: false, precision: 7),
                        Interval = c.Int(nullable: false),
                        Status = c.Boolean(nullable: false),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        ModifiedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.ScheduledFrequencyId)
                .ForeignKey("dbo.Applications", t => t.ApplicationId, cascadeDelete: true)
                .Index(t => t.ApplicationId);
            
            CreateTable(
                "dbo.ScheduledTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AppSchedulerId = c.Int(nullable: false),
                        Weekly = c.String(),
                        Monthly = c.Int(),
                        Yearly = c.DateTime(),
                        Status = c.Boolean(nullable: false),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        ModifiedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SmtpDetails",
                c => new
                    {
                        SmtpDetailId = c.Int(nullable: false, identity: true),
                        SmtpHost = c.String(),
                        SmtpUser = c.String(),
                        Password = c.String(),
                        Name = c.String(),
                        Status = c.Boolean(nullable: false),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        ModifiedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.SmtpDetailId);
            
            CreateTable(
                "dbo.SystemSettings",
                c => new
                    {
                        SystemSettingId = c.Int(nullable: false, identity: true),
                        Key = c.String(),
                        Value = c.String(),
                        Label = c.String(),
                        Status = c.Boolean(nullable: false),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        ModifiedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.SystemSettingId);
            
            CreateTable(
                "dbo.UploadFiles",
                c => new
                    {
                        UploadFileId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        ApplicationId = c.Int(nullable: false),
                        ComponentId = c.Int(nullable: false),
                        FileInputPath = c.String(nullable: false),
                        InputFileMask = c.String(nullable: false),
                        IsArchiveOutputRequired = c.Boolean(nullable: false),
                        ArchiveOutputPath = c.String(),
                        ArchiveFileTransferSettingId = c.Int(nullable: false),
                        IsMoveFileRequired = c.Boolean(nullable: false),
                        MoveFileExpression = c.String(),
                        ArchiveFileExpression = c.String(),
                        MoveFilePath = c.String(),
                        MoveFileTransferSettingId = c.Int(nullable: false),
                        Description = c.String(),
                        Status = c.Boolean(nullable: false),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        ModifiedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.UploadFileId)
                .ForeignKey("dbo.Applications", t => t.ApplicationId, cascadeDelete: true)
                .ForeignKey("dbo.Components", t => t.ComponentId, cascadeDelete: true)
                .Index(t => t.ApplicationId)
                .Index(t => t.ComponentId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UploadFiles", "ComponentId", "dbo.Components");
            DropForeignKey("dbo.UploadFiles", "ApplicationId", "dbo.Applications");
            DropForeignKey("dbo.ScheduledFrequencies", "ApplicationId", "dbo.Applications");
            DropForeignKey("dbo.RunComponentStatus", "RunNumberId", "dbo.RunDetails");
            DropForeignKey("dbo.RunComponentStatus", "ComponentStatusId", "dbo.ComponentStatus");
            DropForeignKey("dbo.RunComponentStatus", "ComponentId", "dbo.Components");
            DropForeignKey("dbo.RawFiles", "RunNumberId", "dbo.RunDetails");
            DropForeignKey("dbo.ProcSessions", "ProcStatus", "dbo.JobStatus");
            DropForeignKey("dbo.ProcSessions", "ApplicationId", "dbo.Applications");
            DropForeignKey("dbo.EmailTrackings", "RunNumberId", "dbo.RunDetails");
            DropForeignKey("dbo.RunDetails", "RunNumberStatusId", "dbo.RunNumberStatus");
            DropForeignKey("dbo.RunDetails", "ApplicationId", "dbo.Applications");
            DropForeignKey("dbo.EmailTrackings", "EmailTemplateId", "dbo.EmailTemplates");
            DropForeignKey("dbo.DeliveryEmailSettings", "ApplicationId", "dbo.Applications");
            DropForeignKey("dbo.ComponentOutputLocations", "ComponentId", "dbo.Components");
            DropForeignKey("dbo.ComponentOutputLocations", "ApplicationId", "dbo.Applications");
            DropForeignKey("dbo.ComponentInputLocations", "ComponentId", "dbo.Components");
            DropForeignKey("dbo.ComponentInputLocations", "ApplicationId", "dbo.Applications");
            DropForeignKey("dbo.ComponentConfigs", "ApplicationComponentId", "dbo.ApplicationComponents");
            DropForeignKey("dbo.ApplicationSmtps", "ApplicationId", "dbo.Applications");
            DropForeignKey("dbo.ApplicationFiles", "ApplicationId", "dbo.Applications");
            DropForeignKey("dbo.ApplicationConfigFiles", "ApplicationId", "dbo.Applications");
            DropForeignKey("dbo.ApplicationComponents", "ComponentId", "dbo.Components");
            DropForeignKey("dbo.TriggerandStatusFiles", "ComponentId", "dbo.Components");
            DropForeignKey("dbo.ApplicationComponents", "ApplicationId", "dbo.Applications");
            DropForeignKey("dbo.Applications", "FileTransferSettingId", "dbo.FileTransferSettings");
            DropForeignKey("dbo.FileTransferSettings", "QueueTypeId", "dbo.QueueTypes");
            DropForeignKey("dbo.Applications", "ClientId", "dbo.Clients");
            DropForeignKey("dbo.Clients", "UserId", "dbo.Users");
            DropIndex("dbo.UploadFiles", new[] { "ComponentId" });
            DropIndex("dbo.UploadFiles", new[] { "ApplicationId" });
            DropIndex("dbo.ScheduledFrequencies", new[] { "ApplicationId" });
            DropIndex("dbo.RunComponentStatus", new[] { "ComponentStatusId" });
            DropIndex("dbo.RunComponentStatus", new[] { "ComponentId" });
            DropIndex("dbo.RunComponentStatus", new[] { "RunNumberId" });
            DropIndex("dbo.RawFiles", new[] { "RunNumberId" });
            DropIndex("dbo.ProcSessions", new[] { "ProcStatus" });
            DropIndex("dbo.ProcSessions", new[] { "ApplicationId" });
            DropIndex("dbo.RunDetails", new[] { "RunNumberStatusId" });
            DropIndex("dbo.RunDetails", new[] { "ApplicationId" });
            DropIndex("dbo.EmailTrackings", new[] { "RunNumberId" });
            DropIndex("dbo.EmailTrackings", new[] { "EmailTemplateId" });
            DropIndex("dbo.DeliveryEmailSettings", new[] { "ApplicationId" });
            DropIndex("dbo.ComponentOutputLocations", new[] { "ApplicationId" });
            DropIndex("dbo.ComponentOutputLocations", new[] { "ComponentId" });
            DropIndex("dbo.ComponentInputLocations", new[] { "ApplicationId" });
            DropIndex("dbo.ComponentInputLocations", new[] { "ComponentId" });
            DropIndex("dbo.ComponentConfigs", new[] { "ApplicationComponentId" });
            DropIndex("dbo.ApplicationSmtps", new[] { "ApplicationId" });
            DropIndex("dbo.ApplicationFiles", new[] { "ApplicationId" });
            DropIndex("dbo.ApplicationConfigFiles", new[] { "ApplicationId" });
            DropIndex("dbo.TriggerandStatusFiles", new[] { "ComponentId" });
            DropIndex("dbo.FileTransferSettings", new[] { "QueueTypeId" });
            DropIndex("dbo.Clients", new[] { "UserId" });
            DropIndex("dbo.Applications", new[] { "FileTransferSettingId" });
            DropIndex("dbo.Applications", new[] { "ClientId" });
            DropIndex("dbo.ApplicationComponents", new[] { "ComponentId" });
            DropIndex("dbo.ApplicationComponents", new[] { "ApplicationId" });
            DropTable("dbo.UploadFiles");
            DropTable("dbo.SystemSettings");
            DropTable("dbo.SmtpDetails");
            DropTable("dbo.ScheduledTypes");
            DropTable("dbo.ScheduledFrequencies");
            DropTable("dbo.RunComponentStatus");
            DropTable("dbo.RunArchiveDetails");
            DropTable("dbo.RawFiles");
            DropTable("dbo.Proofs");
            DropTable("dbo.ProofFiles");
            DropTable("dbo.ProcSessions");
            DropTable("dbo.ProcComponents");
            DropTable("dbo.JobStatus");
            DropTable("dbo.GMCCommands");
            DropTable("dbo.ErrorLogs");
            DropTable("dbo.RunNumberStatus");
            DropTable("dbo.RunDetails");
            DropTable("dbo.EmailTrackings");
            DropTable("dbo.EmailTokens");
            DropTable("dbo.EmailTemplates");
            DropTable("dbo.EmailStatus");
            DropTable("dbo.DeliveryEmailSettings");
            DropTable("dbo.ContactInfoes");
            DropTable("dbo.ComponentStatus");
            DropTable("dbo.ComponentOutputLocations");
            DropTable("dbo.ComponentInputLocations");
            DropTable("dbo.ComponentConfigs");
            DropTable("dbo.ClientSmtps");
            DropTable("dbo.ApplicationSmtps");
            DropTable("dbo.ApplicationFiles");
            DropTable("dbo.ApplicationConfigFiles");
            DropTable("dbo.TriggerandStatusFiles");
            DropTable("dbo.Components");
            DropTable("dbo.QueueTypes");
            DropTable("dbo.FileTransferSettings");
            DropTable("dbo.Users");
            DropTable("dbo.Clients");
            DropTable("dbo.Applications");
            DropTable("dbo.ApplicationComponents");
            DropTable("dbo.AppCommandArguments");
        }
    }
}
