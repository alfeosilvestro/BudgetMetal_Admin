using System;
using System.ComponentModel.DataAnnotations;

namespace Com.BudgetMetal.DB
{
    public class GenericEntity
    {
        [Key]
        public int Id
        {
            get;
            set;
        }

        public DateTime CreatedDate
        {
            get;
            set;
        }

        public DateTime UpdatedDate
        {
            get;
            set;
        }

        public string CreatedBy
        {
            get;
            set;
        }

        public string UpdatedBy
        {
            get;
            set;
        }

        public bool IsActive
        {
            get;
            set;
        }

        public string Version
        {
            get;
            set;
        }

        public enum InitiateMode { NewRecord, EditRecord }


        /// <summary>
        /// For Entity Framework to use
        /// </summary>
        public GenericEntity()
        { }

        /// <summary>
        /// For users to initiate record operations
        /// </summary>
        /// <param name="mode">Mode.</param>
        /// <param name="userName">User name.</param>
        public GenericEntity(InitiateMode mode, string userName)
        {
            switch (mode)
            {
                case InitiateMode.NewRecord:
                    this.PrepareNewRecord(userName);
                    break;

                case InitiateMode.EditRecord:
                    this.PrepareUpdateRecord(userName);
                    break;
            }
        }

        public void PrepareNewRecord(string userName)
        {
            this.CreatedDate = this.UpdatedDate = DateTime.Now;
            this.CreatedBy = this.UpdatedBy = userName;
            UpdateVersion();
        }

        public void PrepareUpdateRecord(string userName)
        {
            this.UpdatedDate = DateTime.Now;
            this.UpdatedBy = userName;
            UpdateVersion();
        }

        private void UpdateVersion()
        {
            this.Version = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        }
    }
}

