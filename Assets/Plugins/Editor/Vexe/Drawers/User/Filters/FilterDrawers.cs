﻿using System;
using System.Text.RegularExpressions;
using Vexe.Editor.GUIs;
using Vexe.Editor.Helpers;
using Vexe.Runtime.Extensions;
using Vexe.Runtime.Helpers;
using Vexe.Runtime.Types;

namespace Vexe.Editor.Drawers
{
	public class TextFilter
	{
		private readonly string[] _values;
		private readonly Action<string> _setValue;
        private readonly int _id;
		private string _pattern, _previous;
        private bool _toggle;
        private bool _cachedResult;

        static Func<string, Regex> _getRegex;
        static Regex GetRegex(string pattern)
        {
            if (_getRegex == null)
                _getRegex = new Func<string, Regex>(x => new Regex(x, RegexOptions.IgnoreCase)).Memoize();
            return _getRegex(pattern);
        }

		public TextFilter(string[] values, int id, Action<string> setValue)
            : this(values, id, true, setValue)
        {
        }

		public TextFilter(string[] values, int id, bool initialToggle, Action<string> setValue)
		{
			this._values   = values;
			this._setValue = setValue;
            this._id = RuntimeHelper.CombineHashCodes(id, "Filter");

            var prefs = BetterPrefs.GetEditorInstance();
            _toggle = prefs.Bools.ValueOrDefault(this._id, initialToggle);
            _pattern = prefs.Strings.ValueOrDefault(this._id, "");
		}

        public bool Field(BaseGUI gui, float width)
        {
            bool changed = false;
            if (_toggle)
            {
                gui.BeginCheck();
                var text = gui.Text(_pattern, Layout.sWidth(width));
                if (gui.HasChanged())
                {
                    changed = true;
                    _pattern = text;

                    var prefs = BetterPrefs.GetEditorInstance();
                    prefs.Strings[_id] = _pattern;
                }
            }
            else gui.Text("", Layout.sWidth(5f));

            var buttonStr = _toggle ? "<" : ">";
            if (gui.Button(buttonStr, GUIStyles.None, Layout.sWidth(13f)))
            {
                _toggle = !_toggle;
                var prefs = BetterPrefs.GetEditorInstance();
                prefs.Bools[_id] = _toggle;
                gui.RequestResetIfRabbit();
            }
            return changed;
        }

        public bool IsMatch(string input)
        {
            return IsMatch(input, _pattern);
        }

        public bool IsMatch(string input, string pattern)
        {
            try
            {
                if (_previous == pattern)
                    return _cachedResult;

                _previous = pattern;

                var regex = GetRegex(pattern);
                _cachedResult = regex.IsMatch(input);
                return _cachedResult;
            }
            catch
            {
                return false;
            }
        }

        public string Process(string pattern)
        {
            string match = null;
            for (int i = 0; i < _values.Length; i++)
            {
                var x = _values[i];
                if (IsMatch(x, pattern))
                {
                    match = x;
                    break;
                }
            }
            return match ?? pattern;
        }

		public void OnGUI(BaseGUI gui)
        {
            OnGUI(gui, 50f);
        }

        public void OnGUI(BaseGUI gui, float width)
		{
            bool changed = Field(gui, width);
			if (changed)
			{
				string match = Process(_pattern);
				if (match != _pattern)
					_setValue(match);
			}
		}
    }

	public abstract class FilterDrawer<T, A> : CompositeDrawer<T, A> where A : CompositeAttribute
	{
		private TextFilter filter;

		protected override void Initialize()
		{
			filter = new TextFilter(GetValues(), id, SetValue);
		}

		public override void OnRightGUI()
		{
			filter.OnGUI(gui);
		}

		protected abstract string[] GetValues();
		protected abstract void SetValue(string value);
	}

	public class FilterEnumDrawer : FilterDrawer<Enum, FilterEnumAttribute>
	{
		protected override void SetValue(string value)
		{
			memberValue = Enum.Parse(memberType, value) as Enum;
		}

		protected override string[] GetValues()
		{
			return Enum.GetNames(memberType);
		}
	}

	public class FilterTagsDrawer : FilterDrawer<string, FilterTagsAttribute>
	{
		protected override string[] GetValues()
		{
			return EditorHelper.GetTags();
		}

		protected override void SetValue(string value)
		{
			memberValue = value;
		}
	}
}