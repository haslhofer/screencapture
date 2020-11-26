using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace screencapture
{
    public partial class ControllerUx : Form
    {
        private List<FloaterUx> _Floaters = new List<FloaterUx>();
        private Button score0 = new Button();
        private Button score1 = new Button();
        private Label conf = new Label();
        private ListBox _entities = new ListBox();
        private int _Counter = 1;

        private List<string> _names = new List<string>();

        private AssessmentResult _AssessmentResult;
        public ControllerUx()
        {

            InitializeComponent();
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(300, 300);
            this.Opacity = 0.8;
            this.TopMost = true;

            score0.Text = "none";
            score0.Width = 300;
            score0.Height = 50;
            score0.ForeColor = Color.DarkOrange;
            score0.Font = new Font("Tahoma", 20, FontStyle.Bold);
            score0.Click += score0_click;
            score0.Visible = false;

            score1.Text = "none";

            score1.Width = 300;
            score1.Location = new Point(0, 50);
            score1.Click += score1_click;
            score1.Visible = false;

            //Confidence
            conf.Location = new Point(0, 70);
            conf.Visible = false;

            //Entities
            _entities.Location = new Point(0, 0);
            _entities.Size = new Size(400, 800);
            _entities.SelectedValueChanged += new EventHandler(ListBox1_SelectedValueChanged);


            this.Controls.Add(score0);
            this.Controls.Add(score1);
            this.Controls.Add(conf);
            this.Controls.Add(_entities);



        }
        private async void ListBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            if (_entities.SelectedIndex != -1)
            {
                string entity = (string)(_entities.Items[_entities.SelectedIndex]);
                await OneNoteCapture.AppendImage(_AssessmentResult.PathToImage, Configurator.GetPageIdFromHashTag(entity));
                
            }
        }


        void score0_click(Object sender, EventArgs e)
        {
            UpdateText(0);
        }

        void score1_click(Object sender, EventArgs e)
        {
            UpdateText(1);
        }

        private async void UpdateText(int index)
        {
            string hashTag = _AssessmentResult.ConfidenceScoreResults[index].Hashtag;
            LanguageModel.UpdateHashtag(hashTag, _AssessmentResult.CapturedText);
            await OneNoteCapture.AppendImage(_AssessmentResult.PathToImage, Configurator.GetPageIdFromHashTag(hashTag));
        }



        void myButton_Click(Object sender, EventArgs e)
        {
            int startPos = _Counter * 10;
            _Counter++;

            FloaterUx ux = new FloaterUx(startPos, startPos, startPos + 100, startPos + 100, "HelloWorld");
            ux.Show();
            _Floaters.Add(ux);
        }

        public void SetConfidence(AssessmentResult r)
        {
            string c0 = "<Empty>";
            string c1 = "<Empty>";
            string confidenceText = "unknown";

            if (r.RecognizedEntities == null)
            {
                r.RecognizedEntities = new List<NerResponse>();
            }

            _AssessmentResult = r;
            if (r.ConfidenceScoreResults.Count > 0)
            {
                c0 = "OneNote: " + r.ConfidenceScoreResults[0].GetDebug();
                confidenceText = r.ConfidenceScoreResults[0].Confidence.ToString();
            }
            if (r.ConfidenceScoreResults.Count > 1)
            {
                c1 = r.ConfidenceScoreResults[1].GetDebug();
            }

            //Update List
            foreach (var anItem in r.RecognizedEntities)
            {
                if (!_names.Contains(anItem.text))
                {
                    _names.Add(anItem.text);
                }
            }
        



            if (this.InvokeRequired)
            {
                InvokeUI(() =>
                {
                    score0.Text = c0;
                    score1.Text = c1;
                    conf.Text = confidenceText;

                    _entities.Items.Clear();

                    var sorted = from x in _names orderby x select x;

                    foreach (var aNer in sorted)
                    {
                        _entities.Items.Add(aNer);
                    }


                });
            }
            else
            {
                score0.Text = c0;
                score1.Text = c1;
                conf.Text = confidenceText;
                

                _entities.Items.Clear();

                    var sorted = from x in _names orderby x select x;

                    foreach (var aNer in sorted)
                    {
                        _entities.Items.Add(aNer);
                    }

            }


        }

        public void DeleteFloaters()
        {
            foreach (var aFloater in _Floaters)
            {
                if (this.InvokeRequired)
                {
                    InvokeUI(() =>
                    {
                        aFloater.Close();
                    });
                }
                else
                {
                    aFloater.Close();
                }

            }
            _Floaters = new List<FloaterUx>();
        }

        public void AddFloater(int Left, int Top, int Right, int Bottom, string text)
        {
            if (this.InvokeRequired)
            {
                InvokeUI(() =>
                {

                    FloaterUx ux = new FloaterUx(Left, Top, Right, Bottom, text);
                    _Floaters.Add(ux);
                    ux.Show();



                });
            }
            else
            {
                FloaterUx ux = new FloaterUx(Top, Left, Bottom, Right, text);
                _Floaters.Add(ux);
                ux.Show();

                ;
            }

        }

        private void InvokeUI(Action a)
        {
            this.BeginInvoke(new MethodInvoker(a));
        }





    }
}
