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
	class CostInterval
	{
		String _name;
		TimeSpan _from = new TimeSpan(0);
		TimeSpan _to = new TimeSpan(0);
		int _weekDays = 0; // dni bitove pole.
		//00000000
		// nspssup
		List<DateTime> _days = new List<DateTime>(); // sviatky?
		List<DateTime> _daysNeg = new List<DateTime>(); // sviatky?

		public TimeSpan from
		{
			set { _from = value; }
			get { return _from; }
		}

		public TimeSpan to
		{
			set { _to = value; }
			get { return _to; }
		}

		public String name
		{
			get { return _name; }
		}

		public CostInterval(String name, TimeSpan from, TimeSpan to)
		{
			_name = name;
			_from = from;
			_to = to;
		}

		public void addDay(DateTime day, String type)
		{
			if (type == "neg")
				_daysNeg.Add(day);
			else
				_days.Add(day);
		}

		public bool isInInterval(DateTime dateTime)
		{
			// Nieje to jeden s vymenovanych dni.
			//Boolean isInNegetive = false;
			//Boolean isInNormal = true;

			if (_days.Count > 0)
			{
				//isInNormal = false;
				foreach (DateTime day in _days)
					if (day.Month == dateTime.Month && day.Day == dateTime.Day)
					{
						return true;
//						isInNormal = true;
//						break;
					}
			}

			if (_daysNeg.Count > 0)
			{
				//isInNegetive = false;
				foreach (DateTime day in _daysNeg)
					if (day.Month == dateTime.Month && day.Day == dateTime.Day)
					{
						return false;
//						isInNegetive = true;
						//break;
					}
			}

//			if (!isInNormal || isInNegetive)
//				return false;

			// Nieje to spravny den v tyzdni.
			if (_weekDays >0 && ((_weekDays & getWeekDayPos(dateTime.DayOfWeek)) == 0))
				return false;

			// Nieje to spravna hodina.
			if (_from < _to && (dateTime.TimeOfDay < _from || dateTime.TimeOfDay > _to))
				return false;
			// -----------------------------------
			// ****from              to***********

			if (_from > _to && (dateTime.TimeOfDay > _to && dateTime.TimeOfDay < _from))
				return false;
			// -----------------------------------
			//     to*******************from


			return true;
		}

		public void addWeekDay(DayOfWeek weekDay)
		{
			int pos = getWeekDayPos(weekDay);
			_weekDays |= pos;
		}

		private int getWeekDayPos(DayOfWeek weekDay)
		{
			int pos;
			switch (weekDay)
			{
				case DayOfWeek.Monday:
					pos = 1;
					break;
				case DayOfWeek.Tuesday:
					pos = 2;
					break;
				case DayOfWeek.Wednesday:
					pos = 4;
					break;
				case DayOfWeek.Thursday:
					pos = 8;
					break;
				case DayOfWeek.Friday:
					pos = 16;
					break;
				case DayOfWeek.Saturday:
					pos = 32;
					break;
				case DayOfWeek.Sunday:
					pos = 64;
					break;
				default:
					pos = 128;
					break;
			}
			return pos;
		}
	}
}
