using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;


namespace PaymenrOrderAPI.Models
{
    public class PaymentOrder
    {


        [Key]
        public Guid OriginatorConversationId { get; set; }
        [Required]
        [StringLength(20)]
        [DisplayName("Name")]
        public string RemitterName { get; set; }
        [Required]
        [StringLength(10)]
        [DisplayName("Address")]
        public string RemitterAddress { get; set; }

        [Required]
        [DisplayName("Phone Number")]
        public int RemitterPhoneNumber { get; set; }
        [Required]
        [DisplayName("Id Type")]
        public string RemitterIdType { get; set; }
        [Required]
        [DisplayName("Id Number")]
        public int RemitterIdNumber { get; set; }

        public string Country { get; set; }

        [DisplayName("Currency")]
        public string Ccy { get; set; }
        public string FinancialInstituion { get; set; }
        public string SourceOfFunds { get; set; }
        public string PrincipalActivity { get; set; }
        [Required]
        [DisplayName(" Receipient Name")]
        public string RecepientName { get; set; }
        [Required]
        [DisplayName(" Receipient Phone Number")]
        public int RecepientPhoneNumber { get; set; }
        public string PrimaryAccountNumber { get; set; }
        public double Amount { get; set; }
        public Guid RouteId { get; set; }
        public Guid SystemTraceAuditNumber { get; set; }
        public string Reference { get; set; }
    }

}