using System;
using Avalonia;
using Avalonia.Input;
using Avalonia.Xaml.Interactivity;
using AvaloniaEdit;
using AvaloniaEdit.TextMate;
using TextMateSharp.Grammars;
using TapeCalc.Helpers;
using System.Linq;
using AvaloniaEdit.Document;

namespace TapeCalc.Behaviors
{
    public class CalcEditorBehavior : Behavior<TextEditor>
    {
        private TextEditor _textEditor = null;

        public static readonly StyledProperty<string> TextProperty =
            AvaloniaProperty.Register<CalcEditorBehavior, string>(nameof(Text));

        public string Text
        {
            get => GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            if (AssociatedObject is TextEditor textEditor)
            {
                _textEditor = textEditor;
                _textEditor.TextArea.TextEntering += TextEntering;
                _textEditor.KeyUp += KeyPress;
                _textEditor.TextArea.PointerReleased += PointerPressed;
                _textEditor.InstallTextMate(new RegistryOptions(ThemeName.Dark));
                //_textEditor.TextArea.TextView.LineTransformers.OfType<TextMateColoringTransformer>().FirstOrDefault();

                var LineColorizer = new TapeCalcLineTransformer();
                _textEditor.TextArea.TextView.LineTransformers.Add(LineColorizer);
                this.GetObservable(TextProperty).Subscribe(TextPropertyChanged);
                _textEditor.Document = new TextDocument("Welcome to TapeCalc! \r\n\r\n" +
                    "Use the plus, minus, asterik or forward slash to set an operator (or move to the next line).\r\n" +
                    "Press Enter to total all calculations so far (or move to the next line).");
            }
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            if (_textEditor != null)
            {
                _textEditor.TextArea.TextEntering -= TextEntering;
                _textEditor.KeyUp -= KeyPress;
            }
        }

        private void TextEntering(object sender, TextInputEventArgs eventArgs)
        {
            if (_textEditor != null && _textEditor.Document != null)
            {
                Text = _textEditor.Document.Text;
                var doc = _textEditor.TextArea;

                switch (eventArgs.Text)
                {
                    case "\n":
                        doc.Caret.Column = doc.Document.GetLineByNumber(doc.Caret.Line).Length + 1;


                        var curentLineText = _textEditor.Document.GetText(_textEditor.Document.GetLineByNumber(doc.Caret.Line));
                        if (curentLineText == "")
                        {
                            doc.Document.Insert(_textEditor.CaretOffset, "\r\n");
                        } else
                        {
                            TextDocument newDoc = new TextDocument(TextProcessor.ConvertTextEditorText(Calculations.CalculateValues(TextProcessor.ConvertTextEditorText(_textEditor.Document))));
                            _textEditor.Document = newDoc;

                            doc.Caret.Line = doc.Document.Lines.Count;
                        }


                        eventArgs.Handled = true;
                        break;

                    case "-":
                        if (doc.Caret.Column > 2)
                        {
                            doc.Caret.Column = doc.Document.GetLineByNumber(doc.Caret.Line).Length + 1;

                            doc.Document.Insert(_textEditor.CaretOffset, "\r\n- ");
                            eventArgs.Handled = true;
                        }
                        break;
                    case "+":
                        if (doc.Caret.Column > 2)
                        {
                            doc.Caret.Column = doc.Document.GetLineByNumber(doc.Caret.Line).Length + 1;

                            doc.Document.Insert(_textEditor.CaretOffset, "\r\n+ ");
                            eventArgs.Handled = true;
                        }
                        break;
                    case "*":
                        if (doc.Caret.Column > 2)
                        {
                            doc.Caret.Column = doc.Document.GetLineByNumber(doc.Caret.Line).Length + 1;

                            doc.Document.Insert(_textEditor.CaretOffset, "\r\n* ");
                            eventArgs.Handled = true;
                        }
                        break;
                    case "/":
                        if (doc.Caret.Column > 2)
                        {
                            doc.Caret.Column = doc.Document.GetLineByNumber(doc.Caret.Line).Length + 1;

                            doc.Document.Insert(_textEditor.CaretOffset, "\r\n/ ");
                            eventArgs.Handled = true;
                        }
                        break;
                }
            }
        }

        private void TextPropertyChanged(string text)
        {
            if (_textEditor != null && _textEditor.Document != null && text != null)
            {
                var caretOffset = _textEditor.CaretOffset;
                _textEditor.Document.Text = text;
                _textEditor.CaretOffset = caretOffset;
            }
        }

        private void KeyPress(object sender, KeyEventArgs e)
        {
            //if (_textEditor != null && _textEditor.Document != null)
            //{
            //    var text = _textEditor.Document.Text;

            //    switch (e.Key)
            //    {
            //        case Key.Enter:
            //            _textEditor.Document.Text = Calculations.ProcessKeyStroke(text, true);
            //            break;
            //    }
            //}
        }

        private void PointerPressed(object sender, EventArgs e)
        {
            var text = _textEditor.Document.GetText(_textEditor.Document.GetLineByNumber(1));
            if (text.Contains("Welcome to TapeCalc!"))
            {
                _textEditor.Document = new TextDocument();
            }
        }
    }
}
