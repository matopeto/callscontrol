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

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MinutesCounter
{
    public partial class IntervalPickerForm : Form
    {
        public DateTime from
        {
            get
            {
                return new DateTime(dateFrom.Value.Year, dateFrom.Value.Month, dateFrom.Value.Day, 0, 0, 0);
            }
        }

        public DateTime to
        {
            get
            {
                return new DateTime(dateTo.Value.Year, dateTo.Value.Month, dateTo.Value.Day, 23, 59, 59);
            }
        }

        public IntervalPickerForm(DateTime f, DateTime t)
        {
            InitializeComponent();
            if (f == null || f.Ticks == 0)
                f = DateTime.Today;
            if (t == null || t.Ticks == 0)
                t = DateTime.Today;
            dateFrom.Value = f;
            dateTo.Value = t;

            set_interval_selector();
        }

        private void menuItem1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void set_interval_selector()
        {
            comboBox1.SelectedIndex = 0;
        }

        private void dateFrom_ValueChanged(object sender, EventArgs e)
        {
            set_interval_selector();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 1)
            {
                dateFrom.Value = DateTime.Today;
                dateTo.Value = DateTime.Today;
            }
            else if (comboBox1.SelectedIndex == 2)
            {
                dateFrom.Value = DateTime.Today.AddDays(-1);
                dateTo.Value = DateTime.Today.AddDays(-1);
            }
            else if (comboBox1.SelectedIndex == 3 || comboBox1.SelectedIndex == 4)
            {
                Int16 offset = 0;
                DayOfWeek today = DateTime.Now.DayOfWeek;
                switch (today)
                {
                    case DayOfWeek.Monday:
                        offset = 0;
                        break;
                    case DayOfWeek.Tuesday:
                        offset = -1;
                        break;
                    case DayOfWeek.Wednesday:
                        offset = -2;
                        break;
                    case DayOfWeek.Thursday:
                        offset = -3;
                        break;
                    case DayOfWeek.Friday:
                        offset = -4;
                        break;
                    case DayOfWeek.Saturday:
                        offset = -5;
                        break;
                    case DayOfWeek.Sunday:
                        offset = -6;
                        break;
                }
                if (comboBox1.SelectedIndex == 3)
                {
                    dateFrom.Value = DateTime.Today.AddDays(offset);
                    dateTo.Value = DateTime.Today;
                }
                else
                {
                    dateFrom.Value = DateTime.Today.AddDays(offset - 7);
                    dateTo.Value = DateTime.Today.AddDays(offset - 1);
                }
            }

        }

        private void dateTo_ValueChanged(object sender, EventArgs e)
        {
            set_interval_selector();
        }
    }
}