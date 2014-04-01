//Copyright 2009-2011 Matej Hrincar. All rights reserved.

//Redistribution and use in source and binary forms, with or without modification, are
//permitted provided that the following conditions are met:

//   1. Redistributions of source code must retain the above copyright notice, this list of
//      conditions and the following disclaimer.

//   2. Redistributions in binary form must reproduce the above copyright notice, this list
//      of conditions and the following disclaimer in the documentation and/or other materials
//      provided with the distribution.

//THIS SOFTWARE IS PROVIDED BY Matej Hrincar ``AS IS'' AND ANY EXPRESS OR IMPLIED
//WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
//FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL Matej Hrincar OR
//CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
//CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
//SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON
//ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
//NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF
//ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

//The views and conclusions contained in the software and documentation are those of the
//authors and should not be interpreted as representing official policies, either expressed
//or implied, of Matej Hrincar.

using System;

using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace MinutesCounter
{
	public partial class DefaultForm : Form
	{

		//CallLog _callLog;
		Tariff tariff;
		Boolean detailed = false;
		Boolean lastMonth = false;
		Boolean tariffOK = false;
		Boolean counted = false;
        Boolean fromInterval = false;
        DateTime countFrom;
        DateTime countTo;

		public DefaultForm()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			txtResult.Text = "Loading tariff file. Please wait...";
			menuCount.Enabled = false;
			menuTariffInfo.Enabled = false;
			tariffOK = false;
			Refresh();
			Activate();

			String pwd = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
#if DEBUG
			DebugLog.createLog(pwd + "\\debugLog.txt", false);
#endif
			tariff = new Tariff();
			if (!tariff.loadTariffFile(pwd + "\\tariff.xml"))
			{
				tariffOK = false;
				txtResult.Text = "Error while loading tariff.xml: \r\n" + tariff.error;
			}
			else
			{
				tariffOK = true;
				cmbBillDay.SelectedIndex = tariff.payDay;
				txtResult.Text = tariff.getTariffInfo().Replace("\n", "\r\n");
				if (tariff.payDay != 0)
					menuCount.Enabled = true;
				menuTariffInfo.Enabled = true;
			}
		}

		private void menuItem1_Click(object sender, EventArgs e)
		{
			if (cmbBillDay.SelectedIndex == 0)
				return;

			txtResult.Text = "Counting. Please wait...";
            if (!fromInterval)
            {
                tariff.payDay = cmbBillDay.SelectedIndex;
                tariff.count(cmbBillDay.SelectedIndex, lastMonth);
            }
            else
            {
                tariff.count(countFrom, countTo);
            }

			txtResult.Text = tariff.getResult(detailed).Replace("\n", "\r\n");
			txtResult.Focus();
			counted = true;
		}

		private void menuItem4_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void menuItem3_Click(object sender, EventArgs e)
		{
			AboutFrom form = new AboutFrom();
			form.Show();
		}


		private void checkBox1_CheckStateChanged(object sender, EventArgs e)
		{
			lastMonth = chckLastMonth.Checked;
		}


		private void menuItem6_Click(object sender, EventArgs e)
		{
			detailed = !detailed;
			menuDetailed.Checked = detailed;
			if (counted)
			{
				txtResult.Text = tariff.getResult(detailed).Replace("\n", "\r\n");
				txtResult.Focus();
			}

		}

		private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
		{
			if (!tariffOK)
				return;
            fromInterval = false;
			menuCount.Enabled = cmbBillDay.SelectedIndex != 0;
            if (cmbBillDay.SelectedIndex == 32)
            {
                IntervalPickerForm form = new IntervalPickerForm(countFrom, countTo);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    this.countFrom = form.from;
                    this.countTo = form.to;
                    txtResult.Text = "Interval set to: " + countFrom.ToShortDateString() + " " + countFrom.ToShortTimeString();
                    txtResult.Text += " - " + countTo.ToShortDateString() + " " + countTo.ToShortTimeString();
                    txtResult.Text += "\r\n" + tariff.getTariffInfo().Replace("\n", "\r\n"); ;
                    chckLastMonth.Enabled = false;
                    fromInterval = true;
                }
                form.Close();
            }
            else
            {
                chckLastMonth.Enabled = true;
            }
		}

		private void menuItem2_Click(object sender, EventArgs e)
		{

		}

		private void panelBillDay_GotFocus(object sender, EventArgs e)
		{

		}

		private void txtResult_TextChanged(object sender, EventArgs e)
		{

		}

		private void DefaultForm_Closing(object sender, CancelEventArgs e)
		{
			DebugLog.closeLog();
		}

		private void menuItem1_Click_1(object sender, EventArgs e)
		{
			txtResult.Text = tariff.getTariffInfo().Replace("\n", "\r\n");
		}

		private void label1_ParentChanged(object sender, EventArgs e)
		{

		}
	}
}