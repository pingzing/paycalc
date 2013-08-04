using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PayCalc
{
    public class IncomeInfo
    {
        private decimal hourlyPay;
        public decimal HourlyPay 
        {
            get
            {
                return hourlyPay;
            }
            set
            {
                hourlyPay = value;
            }
        }

        private decimal unemploymentPayment;
        public decimal UnemploymentPayment 
        {
            get
            {
                return unemploymentPayment;
            }
            set
            {
                unemploymentPayment = value;
            }
        }

        decimal taxDeduction;
        public decimal TaxDeduction 
        {
            get
            {
                return taxDeduction;
            }
            set 
            {
                taxDeduction = value;
            }
        }

        private decimal retirementPayment;
        public decimal RetirementPayment 
        { 
            get
            {
                return retirementPayment;
            }
            set
            {
                retirementPayment = value;
            }
        }

        public IncomeInfo()
        {
            HourlyPay = 0;
            UnemploymentPayment = 0;
            TaxDeduction = 0;
            RetirementPayment = 0;
        }

        public IncomeInfo(decimal _hourlyPay, decimal _unemploymentPayment, decimal _taxDeduction, decimal _retirementPayment)
        {
            HourlyPay = _hourlyPay;
            UnemploymentPayment = _unemploymentPayment;
            TaxDeduction = _taxDeduction;
            RetirementPayment = _retirementPayment;
        }
    }
}
