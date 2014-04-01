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
	class Tarification
	{
		String _tarification;
		Int32 _minimumSec;
		Int32 _tarificationSec;
		Int32 _maximumSec;

		public Int32 minimumSec
		{
			get { return _minimumSec; }
		}

		public Int32 maximumSec
		{
			get { return _maximumSec; }
		}

		public Int32 tarificationSec
		{
			get { return _tarificationSec; }
		}

		public String tarificationString
		{
			get { return _minimumSec.ToString() + "/" + _tarificationSec.ToString(); }
		}

		public static Boolean isValidTarification(String t)
		{
			Regex r = new Regex("^[0-9]+/[0-9]+(/[0-9]+)?$");
			if (r.Match(t).Success)
				return true;
			else
				return false;
		}

		public TimeSpan paySeconds(Double totalSeconds)
		{
			TimeSpan payCount;
			if ((_maximumSec != -1) && _maximumSec < totalSeconds)
				totalSeconds = _maximumSec;

			// mensia ako minimalna sadzba?
			if (totalSeconds < _minimumSec)
				payCount = new TimeSpan(0, 0, 0, _minimumSec);
			else
				payCount = new TimeSpan(0, 0, 0, (int)Math.Ceiling(totalSeconds / _tarificationSec) * _tarificationSec);

			return payCount;
		}

		public Tarification(String t)
		{
			if (!isValidTarification(t))
				return;

			string[] parts = t.Split('/');
			_minimumSec = Convert.ToInt32(parts[0]);
			_tarificationSec = Convert.ToInt32(parts[1]);

			_maximumSec = -1;
			if (parts.Length == 3)
				_maximumSec = Convert.ToInt32(parts[2]);

			_tarification = t;
		}
	}
}
