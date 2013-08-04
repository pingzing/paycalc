using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.Globalization;
using System.Xml;
using System.IO.IsolatedStorage;
using System.IO;
using System.Xml.Serialization;

namespace PayCalc
{
    public partial class MainPage : PhoneApplicationPage
    {
        readonly string crncy = CultureInfo.CurrentCulture.NumberFormat.CurrencySymbol.ToString();
        IncomeInfo Income = new IncomeInfo();
        NumberStyles style = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands;
        decimal outNum;
        const string XML_FILE_NAME = "income.xml";

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            //check and see if there's an incomeInfo stored in xml. if so, load it into IncomeInfo
            LoadSavedIncome();
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void SaveIncomeInfo()
        {
            System.Diagnostics.Debug.WriteLine("Saving Income.");
            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
            xmlWriterSettings.Indent = true;

            using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (IsolatedStorageFileStream stream = myIsolatedStorage.OpenFile(XML_FILE_NAME, FileMode.OpenOrCreate))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(IncomeInfo));
                    using (XmlWriter xmlWriter = XmlWriter.Create(stream, xmlWriterSettings))
                    {
                        serializer.Serialize(xmlWriter, Income);
                    }
                }
            }
        }

        private void LoadSavedIncome()
        {
            IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication();
            if (isoStore.FileExists(XML_FILE_NAME))
            {
                try
                {
                    using (isoStore)
                    {
                        using (IsolatedStorageFileStream isoFileStream = isoStore.OpenFile(XML_FILE_NAME, FileMode.Open))
                        {
                            XmlSerializer serializer = new XmlSerializer(typeof(IncomeInfo));
                            Income = (IncomeInfo)serializer.Deserialize(isoFileStream);
                        }
                    }
                    ContentPanel.DataContext = Income;
                    taxDeductionsBox.Text += "%";
                    unemploymentBox.Text += "%";
                    retirementTextBox.Text += "%";
                }                    
                catch(Exception ex)
                {
                    if (ex is System.Xml.XmlException || ex is InvalidOperationException)
                    {
                        System.Diagnostics.Debug.WriteLine("Error loading XML file.");
                        using (isoStore = IsolatedStorageFile.GetUserStoreForApplication())
                        {
                            isoStore.DeleteFile(XML_FILE_NAME);
                        }
                        Income = new IncomeInfo();
                        return;
                    }

                    throw;
                }
            }
        }

        private void ClearText(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            textBox.Text = "";
        }

        private void hourlyPayBox_TextChanged(object sender, TextChangedEventArgs e)
        {            
            TextBox payText = (TextBox)sender;            
            if(Decimal.TryParse(payText.Text, style, CultureInfo.CurrentCulture, out outNum))
            {
                Income.HourlyPay = outNum;
            }
        }

        private void unemploymentBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox unemployText = (TextBox)sender;            
            if (Decimal.TryParse(unemployText.Text, style, CultureInfo.CurrentCulture, out outNum))
            {
                Income.UnemploymentPayment = outNum;                
            }
        }

        private void taxDeductionsBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox taxDeductionText = (TextBox)sender;
            if (Decimal.TryParse(taxDeductionText.Text, style, CultureInfo.CurrentCulture, out outNum))
            {
                Income.TaxDeduction = outNum;
            }
        }

        private void retirementTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox retirementText = (TextBox)sender;
            if (Decimal.TryParse(retirementText.Text, style, CultureInfo.CurrentCulture, out outNum))
            {
                Income.RetirementPayment = outNum;
            }
        }

        private void calculateButton_Click(object sender, RoutedEventArgs e)
        {
            payPerHourBlock.Text = "";
            totalPayBlock.Text = "";

            decimal hours = 0;
            Decimal.TryParse(hoursBox.Text, out hours);
            decimal grossPay = hours * Income.HourlyPay;
            decimal taxVal = grossPay * Income.TaxDeduction / 100;
            decimal unemploymentVal = grossPay * Income.UnemploymentPayment / 100;
            decimal retirementVal = grossPay * Income.RetirementPayment / 100;
            decimal netPay = grossPay - taxVal - unemploymentVal - retirementVal;

            if (hours != 0)
            {
                payPerHourBlock.Text = (netPay / hours).ToString("c");
                totalPayBlock.Text = netPay.ToString("c");
            }
            else 
            {
                payPerHourBlock.Text = hours.ToString("c");
                totalPayBlock.Text = hours.ToString("c");
            }
            payPerHourBlock.Visibility = Visibility.Visible;
            totalPayBlock.Visibility = Visibility.Visible;
        }

        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((Pivot)sender).SelectedIndex == 0)
            {
                SaveIncomeInfo();
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox box = (TextBox)sender;
            box.Text = box.Text + "%";
        }
    }
}