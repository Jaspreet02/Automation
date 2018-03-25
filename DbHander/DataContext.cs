using System.Data.Entity;
using DbHander;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DbHander
{
    public sealed class DataContext : IdentityDbContext<ApplicationUser>
    {
        public DataContext()
           : base("name = ConnectionString")
        {
            //Database.SetInitializer(new CircularReferenceDataInitializer());
               this.Configuration.LazyLoadingEnabled = false;
              this.Configuration.ProxyCreationEnabled = false;
            // this.Configuration.LazyLoadingEnabled = false;
        }
        
        public DbSet<Application> Applications { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<ApplicationFile> ApplicationFiles { get; set; }
        public DbSet<ApplicationSmtp> ApplicationSmtps { get; set; }
        public DbSet<ApplicationComponent> ApplicationComponents { get; set; }        
        public DbSet<AppCommandArgument> AppCommandArguments { get; set; }
        public DbSet<ComponentConfig> ComponentConfigs { get; set; }
        public DbSet<ComponentInputLocation> ComponentInputLocations { get; set; }
        public DbSet<ComponentOutputLocation> ComponentOutputLocations { get; set; }
        public DbSet<ContactInfo> ContactInfos { get; set; }
        public DbSet<DeliveryEmailSetting> DelivaryEmailSettings { get; set; }
        public DbSet<ErrorLog> ErrorLogs { get; set; }
        public DbSet<FileTransferSetting> FileTransferSettings { get; set; }
        public DbSet<GMCCommand> GmcCommands { get; set; }        
        public DbSet<ProcComponent> ProcComponents { get; set; }
        public DbSet<ProofFile> ProofFiles { get; set; }
        public DbSet<RawFile> RawFiles { get; set; }
        public DbSet<RunArchiveDetail> RunArchiveDetails { get; set; }
        public DbSet<RunDetail> RunDetails { get; set; }
        public DbSet<ScheduledType> ScheduledTypes { get; set; }
        public DbSet<SmtpDetail> SmtpDetails { get; set; }
        public DbSet<ClientSmtp> ClientSmtps { get; set; }
        public DbSet<TriggerandStatusFile> TriggerandStatusFiles { get; set; }
        public DbSet<EmailToken> EmailTokens { get; set; }
        public DbSet<EmailTracking> EmailTrackings { get; set; }
        public DbSet<Component> Components { get; set; }
        public DbSet<ProcSession> ProcSessions { get; set; }
        public DbSet<Proof> Proofs { get; set; }
        public DbSet<ApplicationConfigFile> ApplicationConfigFiles { get; set; }
        public DbSet<SystemSetting> SystemSettings { get; set; }
        public DbSet<EmailTemplate> EmailTemplates { get; set; }
        public DbSet<RunComponentStatus> RunComponentStatus { get; set; }
        public DbSet<ScheduledFrequency> ScheduledFrequencies { get; set; }
        public DbSet<UploadFile> UploadFiles { get; set; }
        public DbSet<RunNumberStatus> RunNumberStatuses { get; set; }
        public DbSet<ComponentStatus> ComponentStatuses { get; set; }
        public DbSet<JobStatus> JobStatuses { get; set; }
        public DbSet<QueueType> QueueTypes   { get; set; }
        public DbSet<EmailStatus> EmailStatuses { get; set; }
        public static DataContext Create()
        {
            return new DataContext();
        }

    }
}
