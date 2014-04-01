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

namespace MinutesCounter
{
	class FreeMinutes
	{
		private TimeSpan _totalMinutes;
		private TimeSpan _remainingMinutes;
		private String _name;

		public TimeSpan remainingMinutes
		{
			get { return _remainingMinutes; }
			set { _remainingMinutes = value; }
		}

		public TimeSpan totalMinutes
		{
			get { return _totalMinutes; }
		}

		public String name
		{
			get { return _name; }
		}

		public FreeMinutes(String name, Double minutes)
		{
			_totalMinutes = TimeSpan.FromMinutes(minutes);
			_name = name;
			_remainingMinutes = _totalMinutes;
		}

		public void reset() {
			_remainingMinutes = _totalMinutes;
		}


	}
}
