using System;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

using System.Net.Http;
using System.Threading;
using Newtonsoft.Json.Linq;

namespace Vote
{
    public class сandidate
    {
        public int nomer;
        public string id;
        public string firstname;
        public string secondname;
        public string thirdname;
        public string party;
        public string description;
        public string web;
        public string image;
        public string votes;
        public string status;
        public string date;
        public string selected;
        public string img_url = "https://adlibtech.ru/elections/upload_images/";
        protected string percent;
        public Grid candidat123;
        public Action LoadContent { get; set; }

        public void CalculationPercent(int total)
        {
            this.percent = Math.Round(Convert.ToDouble(this.votes) * 100 / total).ToString();
        }
        private сandidate[] Mas;
        public void GetMas(сandidate[] Mas)
        {
            this.Mas = Mas;
        }
        public string GetLastVote()
        {
            string last = "";
            foreach (var el in Mas)
            {
                if (el.selected != "0")
                    last = el.id;
            }
            return last;
        }
        public void ClearButtonAndText2()
        {
            Thread.Sleep(1000);
            foreach (var el in Mas)
            {
                if (el.selectedButton.ImageSource != null)
                    el.selectedButton.ImageSource = null;

                el.L_secondname.Text = "";
                el.L_firstname.Text = "";
                el.L_votes.Text = "";
                el.L_percent.Text = "";

            }
        }
        public void ClearButton()
        {
            foreach (var el in Mas)
            {
                if (el.selectedButton.ImageSource != null)
                    el.selectedButton.ImageSource = null;
            }
        }

        public TapGestureRecognizer tapRecognizer = new TapGestureRecognizer();
        public TapGestureRecognizer voteСandidate = new TapGestureRecognizer();
        public StackLayout ContentText = new StackLayout();
        public StackLayout ButtonLayout = new StackLayout();

        Label L_secondname = new Label();
        Label L_firstname = new Label();
        Label L_votes = new Label();
        Label L_percent = new Label();

        bool firstStar = true;
        bool firstPrint = true;

        public void PreviewPrint(/*Grid candidats*/)
        {
            Grid candidats = this.candidat123;
            if (firstPrint)
            {
                Image face;
                face = new Image();
                face.Source = img_url + this.image;

                face.GestureRecognizers.Add(tapRecognizer);

                candidats.Children.Add(face, 0, nomer);
            }

            L_secondname.Text = this.secondname;
            L_secondname.FontSize = 22;
            L_secondname.TextColor = Color.Black;

            L_firstname.Text = this.firstname + " " + this.thirdname;
            L_firstname.FontSize = 17;
            L_firstname.TextColor = Color.Black;

            L_votes.Text = "Голосов " + this.votes;
            L_votes.FontSize = 15;
            L_votes.TextColor = Color.Black;

            L_percent.Text = "Процент " + this.percent + "%";
            L_percent.FontSize = 15;
            L_percent.TextColor = Color.Black;

            ContentText.Children.Add(L_secondname);
            ContentText.Children.Add(L_firstname);
            ContentText.Children.Add(L_votes);
            ContentText.Children.Add(L_percent);
            ContentText.GestureRecognizers.Add(tapRecognizer);

            if (firstPrint)
                candidats.Children.Add(ContentText, 1, nomer);
            firstPrint = false;

            selectedButton.GestureRecognizers.Add(voteСandidate);
            selectedButton.Clicked += selectCandidat;

            // Для отрисовки галочки предыдущего выбранного кандидата.
            if (this.GetLastVote() == this.id && this.firstStar)
            {
                selectedButton.ImageSource = ImageSource.FromResource("Vote.select.jpg");
                this.firstStar = false;
            }

            selectedButton.BorderColor = Color.Red;
            selectedButton.BorderWidth = 3;
            selectedButton.BackgroundColor = Color.White;
            selectedButton.HeightRequest = 70;

            ButtonLayout.Children.Add(selectedButton);
            ButtonLayout.GestureRecognizers.Add(voteСandidate);
            candidats.Children.Add(ButtonLayout, 2, nomer);

        }

        public Button selectedButton = new Button();
        async void selectCandidat(object sender, EventArgs e)
        {
            await SendVoteCandidat(this.id);
            LoadContent();
            this.ClearButtonAndText2();
        }
        public async Task<string> SendVoteCandidat(string candidate_id)
        {
            string url = "https://adlibtech.ru/elections/api/addvote.php";
            string DiviceID = "1";
            string DiviceName = "IphoneX";

            var parameters = new StringContent(
                "device_id=" + DiviceID
                + "&device_name=" + DiviceName
                + "&candidate_id=" + candidate_id
                + "&last_id=" + this.GetLastVote()
                , Encoding.UTF8, "application/x-www-form-urlencoded");

            var client = new HttpClient();
            var req = new HttpRequestMessage(HttpMethod.Post, url) { Content = parameters };
            var res = await client.SendAsync(req);

            return "";
        }

    }

    public partial class MainPage : ContentPage
    {
        bool FirstLoadPage = true;
        public MainPage()
        {
            InitializeComponent();
            if (FirstLoadPage)
                Navigation.PushModalAsync(new Loading());
            FirstLoadPage = false;

            logo.Source = ImageSource.FromResource("Vote.vote.png");
            FirstLoad();
        }

        private async void ToInformationCandidate(object sender, EventArgs e, int nomer) // Подробнее
        {
            await Navigation.PushModalAsync(new InformationCandidate(MasCandidate[nomer]));
        }

        сandidate[] MasCandidate;
        public async void FirstLoad()
        {
            string Request = await SendRequest2();
            JArray json = JArray.Parse(Request);
            await load(json);
        }
        public async Task load(JArray json)
        {
            int total_sum = 0;
            int i = 0;
            MasCandidate = new сandidate[json.Count - 1];
            foreach (var el in json)
            {
                if (i >= json.Count - 1)
                {
                    total.Text = "Всего " + el["total"].ToString();
                    break;
                }
                сandidate сandidate1 = new сandidate();
                сandidate1.nomer = i;
                сandidate1.id = el["id"].ToString();
                сandidate1.firstname = el["firstname"].ToString();
                сandidate1.secondname = el["secondname"].ToString();
                сandidate1.thirdname = el["thirdname"].ToString();
                сandidate1.party = el["party"].ToString();
                сandidate1.description = el["description"].ToString();
                сandidate1.web = el["web"].ToString();
                сandidate1.image = el["image"].ToString();
                сandidate1.votes = el["votes"].ToString();
                сandidate1.status = el["status"].ToString();
                сandidate1.date = el["date"].ToString();
                сandidate1.selected = el["selected"].ToString();
                сandidate1.LoadContent = FirstLoad;
                MasCandidate[i] = сandidate1;
                i++;
                // Вычисление общего числа голосов, так-как приходящее значение total из json массива не равно сумме голосов.
                total_sum += Convert.ToInt32(сandidate1.votes);
            }

            foreach (var el in MasCandidate)
            {
                el.GetMas(MasCandidate);
                el.CalculationPercent(total_sum);

                el.candidat123 = candidats;

                el.PreviewPrint();

                el.tapRecognizer.Tapped += (sender, e) =>
                {
                    ToInformationCandidate(sender, e, el.nomer);
                };

            }
            total.Text += " (" + total_sum.ToString() + ")";
        }

        public async Task<string> SendRequest2()
        {
            string DiviceID = "1";
            string DiviceName = "IphoneX";
            string url = "https://adlibtech.ru/elections/api/getcandidates.php";

            var parameters = new StringContent("device_id=" + DiviceID + "&device_name=" + DiviceName, Encoding.UTF8, "application/x-www-form-urlencoded");

            var client = new HttpClient();
            var req = new HttpRequestMessage(HttpMethod.Post, url) { Content = parameters };
            var res = await client.SendAsync(req);

            var bytes = await res.Content.ReadAsByteArrayAsync();

            Encoding encoding = Encoding.GetEncoding("utf-8");
            string data = encoding.GetString(bytes, 0, bytes.Length);

            res.EnsureSuccessStatusCode();

            return data;
        }

    }

}
