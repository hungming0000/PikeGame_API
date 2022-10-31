﻿//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebBO
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class NT_PlantTreeEntities : DbContext
    {
        public NT_PlantTreeEntities()
            : base("name=NT_PlantTreeEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<tbAppContractTypeDoc> tbAppContractTypeDoc { get; set; }
        public virtual DbSet<tbAppSaplingAllocation> tbAppSaplingAllocation { get; set; }
        public virtual DbSet<tbAppSurvey> tbAppSurvey { get; set; }
        public virtual DbSet<tbAppSurveyTable> tbAppSurveyTable { get; set; }
        public virtual DbSet<tbAppSurveyUser> tbAppSurveyUser { get; set; }
        public virtual DbSet<tbContractFruit> tbContractFruit { get; set; }
        public virtual DbSet<tbContractLand> tbContractLand { get; set; }
        public virtual DbSet<tbContractLandCadastral> tbContractLandCadastral { get; set; }
        public virtual DbSet<tbContractLandForest> tbContractLandForest { get; set; }
        public virtual DbSet<tbContractTree> tbContractTree { get; set; }
        public virtual DbSet<tbMeasureBoundaryPile> tbMeasureBoundaryPile { get; set; }
        public virtual DbSet<tbNSAllotDetail> tbNSAllotDetail { get; set; }
        public virtual DbSet<tbNSAllotReceive> tbNSAllotReceive { get; set; }
        public virtual DbSet<tbNSCultivateWorkItem> tbNSCultivateWorkItem { get; set; }
        public virtual DbSet<tbNSCultivateWorkItemTemplate> tbNSCultivateWorkItemTemplate { get; set; }
        public virtual DbSet<tbNSCultivateWorkPeriod> tbNSCultivateWorkPeriod { get; set; }
        public virtual DbSet<tbNSCultivateWorkPeriodTemplate> tbNSCultivateWorkPeriodTemplate { get; set; }
        public virtual DbSet<tbNSCultivateWorkValue> tbNSCultivateWorkValue { get; set; }
        public virtual DbSet<tbNSCultivateWorkValueTemplate> tbNSCultivateWorkValueTemplate { get; set; }
        public virtual DbSet<tbRelAppContractTypeSurvey> tbRelAppContractTypeSurvey { get; set; }
        public virtual DbSet<tbSysUserData> tbSysUserData { get; set; }
        public virtual DbSet<tbTMLandList> tbTMLandList { get; set; }
        public virtual DbSet<tbTMLandListBackup> tbTMLandListBackup { get; set; }
        public virtual DbSet<tbTMLandListRecord> tbTMLandListRecord { get; set; }
        public virtual DbSet<tbTMLandListResurvey> tbTMLandListResurvey { get; set; }
        public virtual DbSet<tbTree> tbTree { get; set; }
        public virtual DbSet<tbTreeFruit> tbTreeFruit { get; set; }
        public virtual DbSet<V_ContractLand> V_ContractLand { get; set; }
        public virtual DbSet<V_ContractLandCadastral> V_ContractLandCadastral { get; set; }
        public virtual DbSet<V_ContractLandForest> V_ContractLandForest { get; set; }
        public virtual DbSet<V_RTUpdateLandPrice> V_RTUpdateLandPrice { get; set; }
        public virtual DbSet<tbRTLandValueTemp> tbRTLandValueTemp { get; set; }
        public virtual DbSet<tbAppSaplingAllocationItem> tbAppSaplingAllocationItem { get; set; }
        public virtual DbSet<V_ContractAccept> V_ContractAccept { get; set; }
    
        public virtual ObjectResult<string> SP_TM_UpdateLandByResurvey(string tMLandListID, string tMLandListResurveyID)
        {
            var tMLandListIDParameter = tMLandListID != null ?
                new ObjectParameter("TMLandListID", tMLandListID) :
                new ObjectParameter("TMLandListID", typeof(string));
    
            var tMLandListResurveyIDParameter = tMLandListResurveyID != null ?
                new ObjectParameter("TMLandListResurveyID", tMLandListResurveyID) :
                new ObjectParameter("TMLandListResurveyID", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("SP_TM_UpdateLandByResurvey", tMLandListIDParameter, tMLandListResurveyIDParameter);
        }
    }
}