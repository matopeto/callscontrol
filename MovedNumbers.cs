﻿//Copyright 2009-2011 Matej Hrincar. All rights reserved.

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
	class MovedNumbers
	{
		class MovedNumber {
			Regex _regex;
			String _where;

			public String where
			{
				get { return _where; }
			}

			public MovedNumber(Regex regex, String where) {
				_regex = regex;
				_where = where;
			}

			public Boolean match(String number) {
				if (_regex.Match(number).Success)
					return true;
				
				return false;
			}

		}


		List<MovedNumber> _movedNumbers = new List<MovedNumber>();

		/**
		 * Prida regularny vyraz do pocitadla
		 * @param regexp     regularny vyraz, ktory chceme pridat.
		 * @param type       pokial je "neg" tak sa vyraz prida do tzv. negovanych vyrazov,
		 *                   t.j cisla ktore nemaju padnut do pocitadla.
		 * @return           true ak sa podarilo pridat false ak sa nepodarilo pridat.
		 */
		public Boolean addRegexp(String regexp, String where)
		{
			try
			{
				_movedNumbers.Add(new MovedNumber(new Regex(regexp), where));
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
		public Boolean addNumber(String number, String where)
		{
			String regexp = Numbers.sanitizeNumberToRegex(number);
			try
			{
				_movedNumbers.Add(new MovedNumber(new Regex(regexp), where));
				return true;
			}
			catch
			{
				return false;
			}
		}

		/**
		 * Vrati true ak cislo pasuje do ineho pocitadla ako je zadane.
		 */
		public String whichCounter(String number)
		{
			foreach (MovedNumber movedNumber in _movedNumbers)
			{
				if (movedNumber.match(number))
					return movedNumber.where;
			}
			return "";
		}


	}
}
