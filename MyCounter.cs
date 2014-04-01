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
using System.Text.RegularExpressions;

namespace MinutesCounter
{
	class MyCounter
	{
		private String _name;                  // Meno pocitadla.
		private String _fromMinutes;           // Z ktorych volnych minut cerpa.
		private Double _VAT;                   // DPH (berie z tarifu)
		private String _currency;              // Mena (berie z tarifu)
		private Boolean _moneyWithVAT;

		private TimeSpan _counter;             // Pocitadlo.
		private TimeSpan _realCounter;         // Realne pocitadlo.
		private Double _money;                 // Prevolane peniaze
		private Int32 _callsCount;             // Pocet hovorov.
		private Int32 _zeroCallsCount;         // Pocitadlo prezvanani.

		List<Regex> _regexps;                  // Regularne vyrazy, ktore patria do pocitadla.
		List<Regex> _negativeRegexps;          // Regularne vyrazy, ktore nepatria do pocitadla.

		Tarification _tarification;          // Tarifikacia.

		String _shortResultString = "";                      // Vystupny retazec v kratkom formate.
		String _longResultString = "";  // Vystupny retazec v dlhom formate.

		Dictionary<String, CounterCost> _costs = new Dictionary<String, CounterCost>(); // Kolo stoji hovor v ktorom cost intervale.

		/**
		 * Nastavi kratky vystupny retazec. 
		 */
		public void setShortResultString(String value)
		{
			_shortResultString = value;
			_shortResultString = Regex.Replace(_shortResultString, @"\n\s*", "").Trim();
			_shortResultString = Regex.Replace(_shortResultString, @"\n|\r", " ").Trim();
		}

		/**
		 * Nastavi dlhy vystupny retazec. 
		 */
		public void setLongResultString(String value)
		{
			_longResultString = value;
			_longResultString = Regex.Replace(_longResultString, @"\n\s*", "").Trim();
			_longResultString = Regex.Replace(_longResultString, @"\n|\r", " ").Trim();
		}

		/**
		 * Zakladny konstruktor.
		 * @param name                   meno pocitadla.
		 * @param tariffication          tarifikacia pre pocitadlo.
		 * @param fromMinutes            z ktorych volnych minut pocitadlo odpocitava.
		 * @param VAT                    DPH (pre zobrazovanie ceny s dph) Prenasa sa rovnaka hodnota z Tafiff.
		 * @param currency               mena. Prenasa sa rovnaka hodnota z Tafiff.
		 */
		public MyCounter(string name, string tariffication, string fromMinutes, Double VAT, String currency, Boolean moneyWithVAT)
		{
			_name = name;
			_money = 0;
			_counter = _realCounter = new TimeSpan(0);

			_regexps = new List<Regex>();
			_negativeRegexps = new List<Regex>();

			_fromMinutes = fromMinutes;
			_currency = currency;
			_VAT = VAT;
			_moneyWithVAT = moneyWithVAT;

			_tarification = new Tarification(tariffication);

			_shortResultString = "{counter-name}: {timer}";                      // Vystupny retazec v kratkom formate.
			_longResultString = "{counter-name}: {timer}{nl}";
			if (tariffication != "1/1")
				_longResultString += "{counter-name} real: {timer-real}{nl}";
			_longResultString += "{counter-name} money: {money}{currency}";  // Vystupny retazec v dlhom formate.

			_costs.Clear();
			resetTimers();
		}

		/**
		 * Pokusi sa pridat cislo do pocitadla a zapocitat dlzku a peniaze.
		 * @param number        cislo na ktore sme dany hovor volali.
		 * @param start         cas zacatia hovoru.
		 * @param end           cas skoncenia hovoru.
		 * @param costInterval  nazov cost intervalu, do ktoreho hovor padol.
		 * @param freeMinutes   pointer na slovnik volnych minut, aby sme mohli od nich odratat minuty.
		 * @param movedNumbers  pointer na prenesene cisla.
		 * 
		 * @return    true ak sa podarilo pridat (t.j. cislo padlo do daneho pocitadla a aspon do nejakeho cost.
		 *            false inac.
		 */
		public Boolean addToCount(String number, DateTime start, DateTime end, String costInterval, ref Dictionary<String, FreeMinutes> freeMinutes, ref MovedNumbers movedNumbers)
		{
			String movedCounter = movedNumbers.whichCounter(number);
			// Je prenesene?
			if (movedCounter != "" && movedCounter != _name)
				return false;

			// Nezhoduje sa to, ideme het.
			if (!match(number) && (movedCounter != _name))
			{
#if DEBUG
				DebugLog.writeToLog("Number: " + number + " not match");
#endif
				return false;
			}

			TimeSpan length = (end - start).Duration();
			Double totalSeconds = length.TotalSeconds;

			// Hovor by nepadol do ani jedneho cost, nepatri do toho pocitadla.
			if (!_costs.ContainsKey(costInterval) && !_costs.ContainsKey(""))
			{
#if DEBUG
				DebugLog.writeToLog("No default cost");
#endif
				return false;
			}

			// Nulove hovory nikde nezapocitavam.
			// Ale uz nemusim pocitat cez dalsie pocitadla.
			if (totalSeconds == 0)
			{
				_zeroCallsCount++;
				return true;
			}

			_callsCount++;

			// Z ktorych volnych minut budeme brat?
			// Bud z hlavnych alebo z tych definovanych v <cost>
			String fromMinutes = _fromMinutes;
			Tarification tarification = _tarification;
			if (_costs.ContainsKey(costInterval))
			{
				if (_costs[costInterval].fromMinutes != "")
					fromMinutes = _costs[costInterval].fromMinutes;
				if (_costs[costInterval].tarification != "")
					tarification = new Tarification(_costs[costInterval].tarification);
			}
			else if (_costs.ContainsKey(""))
			{
				if (_costs[""].fromMinutes != "")
					fromMinutes = _costs[""].fromMinutes;
				if (_costs[""].tarification != "")
					tarification = new Tarification(_costs[""].tarification);
			}

			// Kolko skutocne zaplatime?
			TimeSpan payCount = tarification.paySeconds(totalSeconds);

			// Skutocne prevolane minuty.
			_realCounter = _realCounter.Add(length);

			// Priratame k vysledku pocitadla.
			_counter = _counter.Add(payCount);

			// Odratame z volnych minut.
			Double paySecondes = 0; // Za kolko sekund uz musime zaplatit.


			if (fromMinutes != "")
			{
				if (freeMinutes.ContainsKey(fromMinutes))
				{
					freeMinutes[fromMinutes].remainingMinutes = freeMinutes[fromMinutes].remainingMinutes.Add(-payCount);
					// Prekrocili sme volne minuty.
					if (freeMinutes[fromMinutes].remainingMinutes.TotalSeconds < 0)
					{
						paySecondes = -freeMinutes[fromMinutes].remainingMinutes.TotalSeconds;
						freeMinutes[fromMinutes].remainingMinutes = new TimeSpan(0);
					}
				}
				else
				{
					// TODO Neboli definovane take volne minuty, zapocitam do platenych.
					// Asi by bolo dobre vyhodit nejaky warning.
					paySecondes = payCount.TotalSeconds;
				}
			}
			else
			{
				// Nebolo definovane z ktorych volnych minut berieme. Platime za vsetko.
				paySecondes = payCount.TotalSeconds;
			}

			// priratame k peniazom.
			// Ak matchuje presne
			// pokial je v ramci volnych minut?
			if (paySecondes > 0)
			{
#if DEBUG
				//DebugLog.writeToLog(String.Format("Minute cost for '{0}' is {1}", costInterval, _costs[costInterval]));
#endif
				if (_costs.ContainsKey(costInterval))
					_money += paySecondes / 60 * _costs[costInterval].cost;
				else if (_costs.ContainsKey(""))
					_money += paySecondes / 60 * _costs[""].cost;
			}

			// Vsetko ok vratime true;
			return true;
		}

		/**
		 * Nahradi specialne retazce za dane hodnoty v result stringoch
		 */
		private String sanitizeResultString(String _string)
		{
			if (String.IsNullOrEmpty(_string))
				return "";

			_string = _string.Replace("{timer}", _counter.ToString());
			_string = _string.Replace("{timer-real}", _realCounter.ToString());
			_string = _string.Replace("{currency}", _currency);
			_string = _string.Replace("{tarification}", _tarification.tarificationString);
			if (!_moneyWithVAT)
				_string = _string.Replace("{money}", _money.ToString());
			else
				_string = _string.Replace("{money}", moneyVAT.ToString());

			_string = _string.Replace("{money+VAT}", moneyVAT.ToString());
			_string = _string.Replace("{counter-name}", _name.ToString());
			_string = _string.Replace("{calls-count}", _callsCount.ToString());
			_string = _string.Replace("{nl}", "\n");
			_string = _string.Replace("{new-line}", "\n");

			return _string;
		}

		/**
		 * Vynuluje pocitadla minut a penazi
		 */
		public void resetTimers()
		{
			_counter = new TimeSpan(0);
			_realCounter = new TimeSpan(0);
			_money = 0;
			_callsCount = 0;
			_zeroCallsCount = 0;
		}

		/**
		 * Vrati peniaze aj s danou
		 */
		public double moneyVAT
		{
			get { return this._money + this._money * _VAT; }
		}

		/**
		 * Vrati dlhy result retazec pre dane pocitalo
		 */
		public string getLongResult()
		{
			return sanitizeResultString(this._longResultString);
		}

		/**
		 * Vrati dlhy result retazec pre dane pocitalo
		 */
		public string getShortResult()
		{
			return sanitizeResultString(this._shortResultString);
		}

		/**
		 * Prida cost do pocitadla
		 * @param value     minutova cena pre dany cost.
		 * @param name      meno costintervalu pre ktory to ma platit.
		 */
		public Boolean addCost(Double value, String name, String tarification, String fromMinutes)
		{
			if (!_costs.ContainsKey(name))
			{
				_costs.Add(name, new CounterCost(value, name, fromMinutes, tarification));
				return true;
			}
			else
				return false;
		}

		/**
		 * Prida regularny vyraz do pocitadla
		 * @param regexp     regularny vyraz, ktory chceme pridat.
		 * @param type       pokial je "neg" tak sa vyraz prida do tzv. negovanych vyrazov,
		 *                   t.j cisla ktore nemaju padnut do pocitadla.
		 * @return           true ak sa podarilo pridat false ak sa nepodarilo pridat.
		 */
		public Boolean addRegexp(String regexp, String type)
		{
			try
			{
				if (type == "neg")
					_negativeRegexps.Add(new Regex(regexp));
				else
					_regexps.Add(new Regex(regexp));
				return true;
			}
			catch (ArgumentException)
			{
				return false;
			}
		}

		/**
		 * Prida cislo do pocitadla. Cislo sa musi presne zhodovat.
		 * @param number     cislo, ktory chceme pridat.
		 * @param type       pokial je "neg" tak sa vyraz prida do tzv. negovanych vyrazov,
		 *                   t.j cisla ktore nemaju padnut do pocitadla.
		 * @return           true ak sa podarilo pridat false ak sa nepodarilo pridat.
		 */
		public bool addNumber(string number, String type)
		{
			String regexp = Numbers.sanitizeNumberToRegex(number);

			try
			{
				if (type == "neg")
					_negativeRegexps.Add(new Regex(regexp));
				else
					_regexps.Add(new Regex(regexp));

				return true;
			}
			catch
			{
				return false;
			}
		}

		/**
		 * Vrati prevolane peniaze
		 */
		public double money
		{
			get { return _money; }
		}

		/**
		 * Vrati prevolane minuty
		 */
		public TimeSpan minutes
		{
			get { return _counter; }
		}

		/**
		 * Vrati meno volnych minut z ktorych cerpame
		 */
		public String fromMinutes
		{
			get { return _fromMinutes; }
		}

		/**
		 * Zistuje ci cislo patri do nasho pocitadla.
		 * @param number      testovane cislo
		 * 
		 * @return true ak ano false ak nie.
		 */
		private Boolean match(string number)
		{
			// Ak matchuje s neg. tak vratime false.
			foreach (Regex r in _negativeRegexps)
				if (r.Match(number).Success)
					return false;

			// Ak matchuje s normalnymi vratime true.
			foreach (Regex r in _regexps)
			{
#if DEBUG
				DebugLog.writeToLog("checking number '" + number + "' with: '" + r.ToString() + "'");
#endif
				if (r.Match(number).Success)
				{
#if DEBUG
					DebugLog.writeToLog("Match!!");
#endif
					return true;
				}
				else
				{
#if DEBUG
					DebugLog.writeToLog("No match");
#endif
				}
			}

			// Inac vratime false.
			return false;
		}
	}
}
