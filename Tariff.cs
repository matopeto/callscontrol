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
using System.Text;
using System.Xml;
using OpenNETCF.Phone;
using System.Text.RegularExpressions;
using System.Globalization;
using System.IO;

namespace MinutesCounter
{
	class Tariff
	{
		// Defalut parametre.
		String _name = "Tariff";                      // Meno tarifu.
		String _tarification = "1/1";                // Tarifikacia pre vsetky pocitadla.
		Double _monthlyPay = 0;                       // Mesacna platba
		Double _VAT = 0;                              // DPH.
		String _currency = "€";                       // Mena v ktorej sa budu zobrazovat informacie.
		Int32 _payDay = 1;                            // Zuctovaci den. (0 musi sa nastavit)
		DateTime _countFrom = new DateTime();         // Odkedy a dokedy sa pocita report.
		DateTime _countTo = new DateTime();
		Boolean _moneyWithVAT = true;                 // Ci sa maju zobrazovat ceny s dph alebo bez
		Boolean _hideEmptyCounters = true;            // Ci sa maju skryt prazdne pocitadla v tagoch {all-counters-*}

		// Formatovacie retazce.
		String _formatShort = "{count-from-date} - {count-to-date}{new-line}" +
							  "Bill: {money}{currency}{new-line}" +
							  "Free minutes:{new-line}" +
							  "{all-free-minutes}" +
							  "Counters:{new-line}{all-counters-short}";

		String _formatLong = "{count-from-date} - {count-to-date}{new-line}" +
							  "Bill: {money}{currency}{new-line}" +
							  "Free minutes:{new-line}" +
							  "{all-free-minutes}" +
							  "Counters:{new-line}{all-counters-long}";

		// Viac pocitadiel.
		Dictionary<String, MyCounter> _counters = new Dictionary<String, MyCounter>();

		// Moze byt viac typov volnych minut. Indexacia nazvom.
		//Dictionary<String, TimeSpan> _freeMinutes = new Dictionary<String, TimeSpan>();
		Dictionary<String, FreeMinutes> _freeMinutes = new Dictionary<String, FreeMinutes>();

		// Moze byt viac typov uctovacich intervalov. Indexacia nazvom.
		Dictionary<String, CostInterval> _costIntervals = new Dictionary<String, CostInterval>();

		// Prenesene cisla.
		MovedNumbers _movedNumbers = new MovedNumbers();

		// Objekt na pritupovanie k zoznamu hovorov.
		CallLog _callLog = new CallLog();

		int _logProgress;
		Boolean _lastMonth = false;
		String _error = "";

		public Boolean counting = true;

		public string name
		{
			get { return _name; }
		}

		public string error
		{
			get { return _error; }
		}

		public Boolean lastMonth
		{
			get { return _lastMonth; }
			set { _lastMonth = value; }
		}

		public string tarification
		{
			get { return _tarification; }
		}

		public double monthlyPay
		{
			get { return _monthlyPay; }
		}

		public Int32 payDay
		{
			get { return _payDay; }
			set { this._payDay = value; }
		}

		public double VAT
		{
			get { return _VAT; }
		}


		public Tariff()
		{
		}

		/**
		 * Nastavi odkedy dokedy ma pocitat
		 */
		private void setCountFromTo()
		{
			if (_payDay == 0)
				return;

			DateTime now = System.DateTime.Now;

			_countFrom = now; _countTo = now;
			if (now.Day < _payDay)
			{
				_countFrom = new DateTime(now.Year, now.AddMonths(-1).Month, _payDay);
				_countTo = now;
			}
			else //if (now.Day >= _payDay)
			{
				_countFrom = new DateTime(now.Year, now.Month, _payDay);
				_countTo = now;
			}

			if (_lastMonth)
			{
				_countTo = _countFrom;
				_countFrom = _countFrom.AddMonths(-1);
			}
		}

		public String getTariffInfo()
		{
			String info = "";

			// Calllog info
			info += "Calllog:";
			_callLog.Seek(CallLogSeek.Beginning, 0);
			Int32 callCount = _callLog.Count;
			info += " (" + callCount.ToString() + ")";
			if (_callLog[callCount - 1] != null)
				info += " Last from: " + _callLog[callCount - 1].StartTime.ToShortDateString();
			info += "\n--------------\n";

			setCountFromTo();
			// Basic info
			info += "Tariff: " + _name + "\n";
			if (_payDay != 0)
				info += "Bill day: " + _payDay + "\n";

			info += "Monthly pay: " + _monthlyPay + _currency;
			if (_VAT != 0)
				info += " + " + (_VAT * 100) + "% VAT";
			info += "\n";

			info += "Free minutes:\n";
			foreach (KeyValuePair<String, FreeMinutes> fmin in _freeMinutes)
			{
				info += fmin.Key + ": " + fmin.Value.totalMinutes.ToString() + "\r\n";
			}
			info += "Intervals:\n";
			foreach (KeyValuePair<String, CostInterval> interval in _costIntervals)
			{
				info += "'" + interval.Key + "'\n";
			}

			info += "Counters:\n";
			foreach (KeyValuePair<String, MyCounter> counter in _counters)
			{
				info += "'" + counter.Key + "'\n";
			}
			info += "\n";
			return info;
		}

		private Int32 convertToInt(String value, String name)
		{
			try
			{
				return Int32.Parse(value);
			}
			catch (FormatException)
			{
				throw new TariffException(String.Format("Parameter '{0}' must be Integer", name));
			}
			catch (OverflowException)
			{
				throw new TariffException(String.Format("Parameter '{0}' must be Integer", name));
			}
		}

		private TimeSpan convertToTimeSpan(String value, String name)
		{
			try
			{
				return TimeSpan.Parse(value);
			}
			catch (FormatException)
			{
				throw new TariffException(String.Format("Parameter '{0}' must be Time in format hh:mm", name));
			}
			catch (OverflowException)
			{
				throw new TariffException(String.Format("Parameter '{0}' must be Time in format hh:mm", name));
			}
		}

		private Double convertToDouble(String value, String name)
		{
			NumberFormatInfo provider = new NumberFormatInfo();
			provider.NumberDecimalSeparator = ".";
			try
			{
				return Double.Parse(value, provider);
			}
			catch (FormatException)
			{
				throw new TariffException(String.Format("Parameter '{0}' must be Double value with dot (.) as decimal separator", name));
			}
			catch (OverflowException)
			{
				throw new TariffException(String.Format("Parameter '{0}' must be Double value with dot (.) as decimal separator", name));
			}
		}

		/**
		 * Nacita volne minuty.
		 */
		private void readFreeMinutes(XmlNode node)
		{
			String name = "";
			if (node.Attributes["name"] != null)
				name = node.Attributes["name"].Value;

			Int32 minute = convertToInt(node.InnerText, "free_minutes");
			if (!_freeMinutes.ContainsKey(name))
				_freeMinutes.Add(name, new FreeMinutes(name, minute));
			else
				throw new TariffException(String.Format("Duplicate definition free minutes with name: {0}", name));
		}

		/**
		 * Nacita cost interval.
		 */
		private void readCostInterval(XmlNode node)
		{
			// Default hodnoty.
			String name = "";
			TimeSpan from = new TimeSpan(0), to = new TimeSpan(0);
			String weekDays = "";

			// Zakladne parametre
			// Povinny
			if (node.Attributes["name"] == null || node.Attributes["name"].Value == "")
				throw new TariffException("Attribute 'name' is mandatory in cost_interval");

			name = node.Attributes["name"].Value;

			if (node.Attributes["from"] != null)
				from = convertToTimeSpan(node.Attributes["from"].Value, "from in free_minutes");
			if (node.Attributes["to"] != null)
				to = convertToTimeSpan(node.Attributes["to"].Value, "to in free_minutes");
			if (node.Attributes["weekdays"] != null)
				weekDays = node.Attributes["weekdays"].Value;

			// Vytvorim costinterval.
			if (_costIntervals.ContainsKey(name))
				throw new TariffException(String.Format("Duplicate cost_interval with name {0}", name));

			CostInterval interval = new CostInterval(name, from, to);

			// WeekDays.
			foreach (String weekDay in weekDays.Split(';'))
			{
				switch (weekDay)
				{
					case "Monday":
						interval.addWeekDay(DayOfWeek.Monday);
						break;
					case "Tuesday":
						interval.addWeekDay(DayOfWeek.Tuesday);
						break;
					case "Wendstay":
						interval.addWeekDay(DayOfWeek.Wednesday);
						break;
					case "Thursday":
						interval.addWeekDay(DayOfWeek.Thursday);
						break;
					case "Friday":
						interval.addWeekDay(DayOfWeek.Friday);
						break;
					case "Saturday":
						interval.addWeekDay(DayOfWeek.Saturday);
						break;
					case "Sunday":
						interval.addWeekDay(DayOfWeek.Sunday);
						break;
				}
			}

			// Dni.
			XmlNodeList nodes = node.ChildNodes;
			Int32 day = -1, month = -1;
			foreach (XmlNode node2 in nodes)
			{
				if (node2.NodeType != XmlNodeType.Element || node2.Name != "day")
					continue;

				if (node2.Attributes["day"] != null)
					day = convertToInt(node2.Attributes["day"].Value, "day in cost_interval");
				if (node2.Attributes["month"] != null)
					month = convertToInt(node2.Attributes["month"].Value, "month in cost_interval");
				String type = "normal";
				if (node2.Attributes["type"] != null)
					type = node2.Attributes["type"].Value;

				if (day != -1 && month != -1)
					interval.addDay(new DateTime(2000, month, day), type);
			}
			_costIntervals.Add(name, interval);
		}

		/**
		 * Nacita prenesene cisla.
		 */
		private void readMovedNumbers(XmlNode node)
		{
			XmlNodeList nodes = node.ChildNodes;
			foreach (XmlNode node2 in nodes)
			{
				if (node2.NodeType != XmlNodeType.Element)
					continue;

				switch (node2.Name)
				{
					case "regex":
					case "number":
						if (node2.Attributes["value"] == null)
							throw new TariffException("Argument value is mandatory in regex or number.");
						if (node2.Attributes["counter"] == null)
							throw new TariffException("Argument counter is mandatory in regex or number.");
						if (node2.Name == "regex")
						{
							if (_movedNumbers.addRegexp(node2.Attributes["value"].Value, node2.Attributes["counter"].Value))
								throw new TariffException("Argument value in regex is not valid regular expression");
						}
						else
						{
							_movedNumbers.addNumber(node2.Attributes["value"].Value, node2.Attributes["counter"].Value);
						}

						break;
				}
			}
		}


		/**
		 * Nacita counter
		 */
		private void readCounter(XmlNode node)
		{
			String name = "";
			String fromMinutes = "";
			String tarification = _tarification;

			NumberFormatInfo provider = new NumberFormatInfo();
			provider.NumberDecimalSeparator = ".";

			// Meno je povinne
			if (node.Attributes["name"] == null)
				throw new TariffException("Argument 'name' in 'counter' is mandatory.");
			name = node.Attributes["name"].Value;

			if (node.Attributes["tarification"] != null)
				tarification = node.Attributes["tarification"].Value;

			if (node.Attributes["from_minutes"] != null)
				fromMinutes = node.Attributes["from_minutes"].Value;

			// Vytvorim counter;
			MyCounter counter = new MyCounter(name, tarification, fromMinutes, _VAT, _currency, _moneyWithVAT);

			XmlNodeList nodes = node.ChildNodes;
			foreach (XmlNode node2 in nodes)
			{
				if (node2.NodeType != XmlNodeType.Element)
					continue;

				switch (node2.Name)
				{
					case "cost":
						String period = "";
						Double cost = 0;
						String costTarification = "";
						String costFromMinutes = "";
						cost = convertToDouble(node2.InnerText, "cost in counter");
						if (node2.Attributes["period"] != null)
							period = node2.Attributes["period"].Value;
						else
							period = "";

						if (node2.Attributes["tarification"] != null)
							costTarification = node2.Attributes["tarification"].Value;
						else
							costTarification = "";

						if (node2.Attributes["from_minutes"] != null)
							costFromMinutes = node2.Attributes["from_minutes"].Value;
						else
							costFromMinutes = "";

						counter.addCost(cost, period, costTarification, costFromMinutes);
						break;
					case "regex":
					case "number":
						if (node2.Attributes["value"] == null)
							throw new TariffException("Argument value is mandatory in regex or number.");

						String type = "normal";
						if (node2.Attributes["type"] != null && node2.Attributes["type"].Value == "neg")
							type = "neg";

						if (node2.Name == "regex")
						{
							if (!counter.addRegexp(node2.Attributes["value"].Value, type))
								throw new TariffException("Argument value in regex is not valid regular expression");
						}
						else
						{
							counter.addNumber(node2.Attributes["value"].Value, type);
						}

						break;
					case "format_short":
						counter.setShortResultString(node2.InnerText);
						break;
					case "format_long":
						counter.setLongResultString(node2.InnerText);
						break;

				}
			}

			_counters.Add(name, counter);
		}


		private Int32 readIntNode(XmlElement root, String path, String name, Int32 defaultValue, Boolean mandatory)
		{
			XmlNode node = root.SelectSingleNode(path);
			if (mandatory && node == null)
			{
				throw new TariffException(String.Format("Parameter '{0}' is mandatory", name));
			}
			else if (node != null)
			{
				return convertToInt(node.InnerText, name);
			}
			return defaultValue;
		}

		private Double readDoubleNode(XmlElement root, String path, String name, Double defaultValue, Boolean mandatory)
		{
			XmlNode node = root.SelectSingleNode(path);
			if (mandatory && node == null)
			{
				throw new TariffException(String.Format("Parameter '{0}' is mandatory", name));
			}
			else if (node != null)
			{
				return convertToDouble(node.InnerText, name);
			}
			return defaultValue;
		}


		private String readStringNode(XmlElement root, String path, String name, String defaultValue, Boolean mandatory)
		{
			XmlNode node = root.SelectSingleNode(path);
			if (mandatory && node == null)
			{
				throw new TariffException(String.Format("Parameter '{0}' is mandatory", name));
			}
			else if (node != null)
			{
				return node.InnerText;
			}
			return defaultValue;
		}


		public Boolean loadTariffFile(string fileName)
		{
			try
			{

				XmlDocument doc = new XmlDocument();
				try
				{
					doc.Load(fileName);
				}
				catch (FileNotFoundException)
				{
					throw new TariffException("File: " + fileName + "not found.");
				}

				XmlElement root = doc.DocumentElement;


				if (root.Name == "tariff")
				{
					_name = readStringNode(root, "//tariff/tariff_name", "tariff_name", _name, false);
					_payDay = readIntNode(root, "//tariff/pay_day", "pay_day", _payDay, false);
					_monthlyPay = readDoubleNode(root, "//tariff/monthly_pay", "monthly_pay", _monthlyPay, false);
					_currency = readStringNode(root, "//tariff/currency", "currency", _currency, false);
					_VAT = readDoubleNode(root, "//tariff/vat", "vat", _VAT, false);
					_tarification = readStringNode(root, "//tariff/tarification", "tarification", _tarification, false);
					
					// Nechavam koli spatnej kompatiblite
					_formatShort = readStringNode(root, "//tariff/format_short", "format_short", _formatShort, false);
					_formatLong = readStringNode(root, "//tariff/format_long", "format_long", _formatLong, false);

					_formatShort = readStringNode(root, "//tariff/format/short", "short", _formatShort, false);
					_formatLong = readStringNode(root, "//tariff/format/long", "long", _formatLong, false);
		
					_moneyWithVAT = Convert.ToBoolean(readIntNode(root, "//tariff/format/money_with_vat", "money_with_vat", Convert.ToInt32(_moneyWithVAT), false));
					_hideEmptyCounters = Convert.ToBoolean(readIntNode(root, "//tariff/format/hide_empty_counters", "hide_empty_counters", Convert.ToInt32(_hideEmptyCounters), false));

					_formatShort = Regex.Replace(_formatShort, @"\n\s*", "").Trim();
					_formatShort = Regex.Replace(_formatShort, @"\n|\r", " ").Trim();
					_formatLong = Regex.Replace(_formatLong, @"\n\s*", "").Trim();
					_formatLong = Regex.Replace(_formatLong, @"\n|\r", " ").Trim();

					XmlNodeList nodes;
					nodes = root.SelectNodes("//tariff/cost_period");
					foreach (XmlNode n in nodes)
						readCostInterval(n);

					nodes = root.SelectNodes("//tariff/free_minutes");
					foreach (XmlNode n in nodes)
						readFreeMinutes(n);

					nodes = root.SelectNodes("//tariff/counter");
					foreach (XmlNode n in nodes)
						readCounter(n);

					XmlNode node = root.SelectSingleNode("//tariff/moved_numbers");
					if (node != null)
						readMovedNumbers(node);

					return true;
				}
				else
				{
					throw new TariffException("No root tariff element");
				}
			}
			catch (TariffException ex)
			{
				_error = ex.Message;
				return false;
			}
			catch (FormatException ex)
			{
				_error = ex.ToString();
				return false;
			}
			catch (XmlException ex)
			{
				_error = ex.Message;
				return false;
			}
		}


		String sanitizeTariffOutput(String _string)
		{
			//{tariff-name} ok
			//{monthly-pay} ok
			//{VAT} ok
			//{currency} ok 
			//{tarification} ok 
			//{all-counters-short} ok
			//{all-counters-long} ok
			//{counter-"name"-short} ok
			//{counter-"name"-long} ok 
			//{all-free-minutes} ok
			//{free-minutes-"name"} ok
			//{count-from}   ok
			//{count-to}     ok
			//{count-from-date}   ok
			//{count-to-date}     ok
			//{money} ok
			//{money+VAT} ok
			//{pay-day}
			//{next-pay-day}
			//{in-call-count}
			//{out-call-count}
			//{in-zero-lenght-call-count}
			//{out-zero-lenght-call-count}
			//{in-non-zero-lenght-call-count}
			//{out-non-zero-lenght-call-count}
			//{missed-call-count}
			// -------------------------
			//{free-minutes-name-start}
			//{free-minutes-name-end}
			//{all-free-minutes-start}
			//{all-free-minutes-end}
			//{all-free-minutes-start/end}
			//{all-free-minutes-end/start}


			_string = _string.Replace("{tariff-name}", _name);
			_string = _string.Replace("{monthly-pay}", _monthlyPay.ToString());
			_string = _string.Replace("{VAT}", _VAT.ToString());
			_string = _string.Replace("{currency}", _currency.ToString());
			_string = _string.Replace("{tarification}", _tarification);
			_string = _string.Replace("{count-from}", _countFrom.ToShortDateString() + " " + _countFrom.ToShortTimeString());
			_string = _string.Replace("{count-to}", _countTo.ToShortDateString() + " " + _countTo.ToShortTimeString());
			_string = _string.Replace("{count-from-date}", _countFrom.ToShortDateString());
			_string = _string.Replace("{count-to-date}", _countTo.ToShortDateString());
			_string = _string.Replace("{count-time}", _countFrom.ToShortTimeString());
			_string = _string.Replace("{count-time}", _countTo.ToShortTimeString());
			_string = _string.Replace("{nl}", "\n");
			_string = _string.Replace("{new-line}", "\n");

			// Potrebujem zistit info o freeminutach
			if (_string.IndexOf("{all-free-minutes}") > 0)
			{
				String freeMinutesString = "";
				foreach (KeyValuePair<String, FreeMinutes> freeMinute in _freeMinutes)
				{
					freeMinutesString += freeMinute.Key + ": " + freeMinute.Value.remainingMinutes.ToString() + "\n";
				}
				_string = _string.Replace("{all-free-minutes}", freeMinutesString);
			}

			if (Regex.IsMatch(_string, "{free-minutes-[^}]+}"))
			{
				foreach (KeyValuePair<String, FreeMinutes> freeMinute in _freeMinutes)
				{
					_string = _string.Replace("{free-minutes-" + freeMinute.Key + "}", freeMinute.Value.remainingMinutes.ToString());
				}
			}


			// Potrebujem zistit info o pocitadlach
			if (_string.IndexOf("{all-counters-short}") > 0 ||
				_string.IndexOf("{all-counters-long}") > 0 ||
				_string.IndexOf("{money}") > 0 ||
				_string.IndexOf("{money+VAT}") > 0)
			{
				String tmpStringShort = "";
				String tmpStringLong = "";
				Double money = 0;
				foreach (KeyValuePair<String, MyCounter> counter in _counters)
				{
					// Preskocime prazdne pocitadla.
					if (counter.Value.minutes.TotalSeconds == 0 && _hideEmptyCounters)
						continue;
					tmpStringShort += counter.Value.getShortResult() + "\n";
					tmpStringLong += counter.Value.getLongResult() + "\n";
					money += counter.Value.money;
				}
				String moneyString = money.ToString();
				if (_moneyWithVAT)
					moneyString = (money + money * _VAT).ToString();

				_string = _string.Replace("{money}", moneyString);
				_string = _string.Replace("{money+VAT}", (money + money * _VAT).ToString());
				_string = _string.Replace("{all-counters-short}", tmpStringShort);
				_string = _string.Replace("{all-counters-long}", tmpStringLong);
			}

			if ((Regex.IsMatch(_string, "{counter-([^}]+)-(short)|(long)}")))
			{
				foreach (KeyValuePair<String, MyCounter> counter in _counters)
				{
					_string = _string.Replace("{counter-" + counter.Key + "-short}", counter.Value.getShortResult());
					_string = _string.Replace("{counter-" + counter.Key + "-long}", counter.Value.getLongResult());
				}
			}


			return _string;
		}

		String sanitizeNumber(String number)
		{
			return Regex.Replace(number, "[^0-9pw*#+]", "");
		}

		public int logCount
		{
			get { return _callLog.Count; }
		}
		public int logProgress
		{
			get { return _logProgress; }
		}

		public void count(Int32 Day, Boolean lastMonth)
		{
			_payDay = Day;
			_lastMonth = lastMonth;
			setCountFromTo();
			count();
		}

		public void count(DateTime from, DateTime to)
		{
			_countFrom = from;
			_countTo = to;
			count();
		}

		private void resetAllTimers()
		{
			foreach (KeyValuePair<String, MyCounter> counter in _counters)
				counter.Value.resetTimers();
			foreach (KeyValuePair<String, FreeMinutes> fmin in _freeMinutes)
				fmin.Value.reset();
		}

		private void count()
		{
			// Pocitanie.
#if DEBUG
			DebugLog.writeToLog("From:" + _countFrom.ToString() + " To:" + _countTo.ToString());
#endif
			// Nastavime sa na zaciatok.
			_callLog.Seek(CallLogSeek.Beginning, 0);

			resetAllTimers();

			counting = true;
			//            List<string> hovory = new List<string>();

			foreach (CallLogEntry call in _callLog)
			{
				_logProgress++;

				if (call.StartTime.CompareTo(_countFrom) < 0)
					break;
				// Chcem iba zo zadaneho intervalu a iba ochadzajuce.
				if ((!call.Outgoing) ||
					(call.StartTime.CompareTo(_countFrom) < 0) ||
					(call.EndTime.CompareTo(_countFrom) < 0) ||
					(call.StartTime.CompareTo(_countTo) > 0)
					//(call.EndTime.CompareTo(_countTo) > 0)
					)
				{
					continue;
				}
#if DEBUG
				DebugLog.writeToLog("Call: " + call.Outgoing.ToString() + " " + (call.Number != null ? call.Number : "Unknow number") + " Start: " + call.StartTime.ToString() + " End:" + call.EndTime.ToString() + " length: " + (call.EndTime - call.StartTime).ToString());
#endif
				// Zistime do ktoreho costIntervalu padne dany hovor.
				string interval = "";
				foreach (KeyValuePair<String, CostInterval> costInterval in _costIntervals)
					if (costInterval.Value.isInInterval(call.StartTime))
					{
						interval = costInterval.Key;
						break;
					}

				// Odstranime znaky, ktore nechceme z cisla.
				String number = Numbers.sanitizeNumber(call.Number);

				// A skusime zaratat dany hovor do pocitadla, do ktoreho padne skor tam ho nechame.
				String counterName = "";
				foreach (KeyValuePair<String, MyCounter> counter in _counters)
				{
					if (counter.Value.addToCount(number, call.StartTime, call.EndTime, interval, ref _freeMinutes, ref _movedNumbers))
					{
						counterName = counter.Key;
						break;
					}
				}
#if DEBUG
				DebugLog.writeToLog("----OK: Number: " + number + " Interval: " + interval + " Counter:" + counterName);
#endif
			}
			counting = false;

		}

		public string getResult(Boolean longResult)
		{
			if (longResult)
				return sanitizeTariffOutput(_formatLong);
			else
				return sanitizeTariffOutput(_formatShort);
		}
	}
}
