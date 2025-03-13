using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Windows.Forms;
using System.Drawing;
using MarketCSharp.material;
using MarketCSharp.tool;
using MarketCSharp.Repository;

namespace MarketCSharp.app
{
    public class MarketApp : Form
    {
        private OleDbConnection _conn;
        private Form _root;
        private ComboBox _yearMonthComboBox;
        private Panel _homePanel;
        private Panel _paymentPanel;
        private Panel _insertMarketPanel;
        private Panel _insertBoxPanel;
        private Label _titleLabel;
        private TextBox _yearMonthTextBox;
        private Button _validateButton;
        private Panel _topPanel;
        private Panel _bottomPanel;
        private Panel _canvasPanel; // Remplace PictureBox par Panel
        private List<Market> _markets;
        private ComboBox _ownerSelect;
        private DateTimePicker _dateEntry;
        private TextBox _amountEntry;
        private TextBox _marketLongEntry;
        private TextBox _marketLargEntry;
        private TextBox _marketXEntry;
        private TextBox _marketYEntry;
        private TextBox _marketNomMarketEntry;
        private ComboBox _idMarketSelect;
        private TextBox _boxNumEntry;
        private TextBox _boxLongEntry;
        private TextBox _boxLargEntry;
        private TextBox _boxXEntry;
        private TextBox _boxYEntry;

        public MarketApp()
        {
            _conn = Connexion.GetConnexion();
            this.Text = "Market Management App";
            this.Size = new System.Drawing.Size(1000, 800);
            this.BackColor = System.Drawing.ColorTranslator.FromHtml("#F0F8FF");
            InitializeComponents();
            this.FormClosing += new FormClosingEventHandler(CloseConnection);
            InitHomePage();
            InitPaymentPage();
            InitInsertMarketPage();
            InitInsertBoxPage();
            _paymentPanel.Hide();
            _insertMarketPanel.Hide();
            _insertBoxPanel.Hide();
            _homePanel.Show();
            _homePanel.BringToFront();
        }

        private void InitializeComponents()
        {
            _homePanel = new Panel();
            _homePanel.Dock = DockStyle.Fill;  
            _homePanel.BackColor = System.Drawing.ColorTranslator.FromHtml("#F0F8FF");
            _paymentPanel = new Panel();
            _paymentPanel.Dock = DockStyle.Fill;
            _paymentPanel.BackColor = System.Drawing.ColorTranslator.FromHtml("#F0F8FF");
            _insertMarketPanel = new Panel();
            _insertMarketPanel.Dock = DockStyle.Fill;
            _insertMarketPanel.BackColor = System.Drawing.ColorTranslator.FromHtml("#F0F8FF");
            _insertBoxPanel = new Panel();
            _insertBoxPanel.Dock = DockStyle.Fill;
            _insertBoxPanel.BackColor = System.Drawing.ColorTranslator.FromHtml("#F0F8FF");
            MenuStrip menuBar = new MenuStrip();
            this.Controls.Add(menuBar);
            this.MainMenuStrip = menuBar;
            ToolStripMenuItem homeMenuItem = new ToolStripMenuItem("Home");
            homeMenuItem.Click += new EventHandler(ShowHome);
            menuBar.Items.Add(homeMenuItem);
            ToolStripMenuItem paymentMenuItem = new ToolStripMenuItem("Paiement");
            paymentMenuItem.Click += new EventHandler(ShowPayments);
            menuBar.Items.Add(paymentMenuItem);
            ToolStripMenuItem insertMarketMenuItem = new ToolStripMenuItem("Insérer Market");
            insertMarketMenuItem.Click += new EventHandler(ShowInsertMarket);
            menuBar.Items.Add(insertMarketMenuItem);
            ToolStripMenuItem insertBoxMenuItem = new ToolStripMenuItem("Insérer Box");
            insertBoxMenuItem.Click += new EventHandler(ShowInsertBox);
            menuBar.Items.Add(insertBoxMenuItem);
            this.Controls.Add(_homePanel);
            this.Controls.Add(_paymentPanel);
            this.Controls.Add(_insertMarketPanel);
            this.Controls.Add(_insertBoxPanel);
            _yearMonthComboBox = new ComboBox();
            _yearMonthComboBox.Items.Add("2022-01");
            _yearMonthComboBox.SelectedIndex = 0; 
            _yearMonthComboBox.SelectedIndexChanged += new EventHandler(UpdateTitle);
        }

        private void CloseConnection(object sender, FormClosingEventArgs e)
        {
            if (_conn != null)
            {
                _conn.Close();
            }
        }

        private void UpdateTitle(object sender, EventArgs e)
        {
            if (_yearMonthComboBox.SelectedItem != null)
            {
                string dateValue = _yearMonthComboBox.SelectedItem.ToString();
                _titleLabel.Text = $"Situation de Marché au mois de {dateValue}";
            }
        }

        private void ShowHome(object sender, EventArgs e)
        {
            _paymentPanel.Hide();
            _insertMarketPanel.Hide();
            _insertBoxPanel.Hide();
            _homePanel.Show();
            _homePanel.BringToFront();
        }

        private void ShowPayments(object sender, EventArgs e)
        {
            _homePanel.Hide();
            _insertMarketPanel.Hide();
            _insertBoxPanel.Hide();
            _paymentPanel.Show();
            _paymentPanel.BringToFront();
        }

        private void ShowInsertMarket(object sender, EventArgs e)
        {
            _homePanel.Hide();
            _paymentPanel.Hide();
            _insertBoxPanel.Hide();
            _insertMarketPanel.Show();
            _insertMarketPanel.BringToFront();
        }

        private void ShowInsertBox(object sender, EventArgs e)
        {
            _homePanel.Hide();
            _paymentPanel.Hide();
            _insertMarketPanel.Hide();
            _insertBoxPanel.Show();
            _insertBoxPanel.BringToFront();
        }

        private void InitHomePage()
        {
            // Nettoyer le panel
            _homePanel.Controls.Clear();
            
            // Créer un panel pour le titre et la sélection de date en haut
            Panel headerPanel = new Panel();
            headerPanel.Dock = DockStyle.Top;
            headerPanel.Height = 100;
            headerPanel.Padding = new Padding(10);
            _homePanel.Controls.Add(headerPanel);
            
            _titleLabel = new Label
            {
                Text = "Situation de Marché au mois de",
                Font = new System.Drawing.Font("Arial", 16, System.Drawing.FontStyle.Bold),
                AutoSize = true,
                Location = new System.Drawing.Point(10, 10)
            };
            headerPanel.Controls.Add(_titleLabel);
            
            // Panel pour la sélection de date
            Panel datePanel = new Panel
            {
                Location = new System.Drawing.Point(10, 50),
                AutoSize = true
            };
            headerPanel.Controls.Add(datePanel);
            
            Label dateLabel = new Label
            {
                Text = "Date:",
                AutoSize = true,
                Location = new System.Drawing.Point(0, 3)
            };
            datePanel.Controls.Add(dateLabel);
            
            _yearMonthTextBox = new TextBox
            {
                Text = "2022-01",
                Width = 100,
                Location = new System.Drawing.Point(50, 0)
            };
            _yearMonthTextBox.TextChanged += new EventHandler((s, e) => {
                if (_yearMonthComboBox.Items.Contains(_yearMonthTextBox.Text))
                {
                    _yearMonthComboBox.SelectedItem = _yearMonthTextBox.Text;
                }
                else if (_yearMonthTextBox.Text.Length == 7) // Format YYYY-MM
                {
                    if (!_yearMonthComboBox.Items.Contains(_yearMonthTextBox.Text))
                    {
                        _yearMonthComboBox.Items.Add(_yearMonthTextBox.Text);
                    }
                    _yearMonthComboBox.SelectedItem = _yearMonthTextBox.Text;
                }
            });
            datePanel.Controls.Add(_yearMonthTextBox);
            
            _validateButton = new Button
            {
                Text = "Valider",
                BackColor = System.Drawing.ColorTranslator.FromHtml("#5A9"),
                ForeColor = System.Drawing.Color.Black,
                Font = new System.Drawing.Font("Arial", 8),
                FlatStyle = FlatStyle.Flat,
                Location = new System.Drawing.Point(160, 0),
                Width = 100
            };
            _validateButton.Click += new EventHandler(ValidateDate);
            datePanel.Controls.Add(_validateButton);
            
            // Panel pour le canvas des marchés - Utiliser un Panel au lieu d'un PictureBox
            _canvasPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = System.Drawing.Color.White,
                AutoScroll = true // Permet de scroller si le contenu est plus grand que le panel
            };
            _homePanel.Controls.Add(_canvasPanel);
            
            // Charger les marchés
            _markets = Market.GetMarkets(_conn);
            
            // Mettre à jour le titre
            UpdateTitle(null, null);
            
            // Afficher les marchés
            DisplayMarkets();
        }

        private void InitPaymentPage()
        {
            // Nettoyer le panel
            _paymentPanel.Controls.Clear();
            
            // Layout principal avec padding
            _paymentPanel.Padding = new Padding(20);
            
            Label paymentLabel = new Label
            {
                Text = "Faire un paiement",
                Font = new System.Drawing.Font("Arial", 16, System.Drawing.FontStyle.Bold),
                AutoSize = true,
                Location = new System.Drawing.Point(20, 20)
            };
            _paymentPanel.Controls.Add(paymentLabel);

            // Tableau pour organiser les contrôles
            TableLayoutPanel table = new TableLayoutPanel
            {
                Location = new System.Drawing.Point(20, 70),
                ColumnCount = 2,
                RowCount = 4,
                AutoSize = true,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None
            };
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));
            _paymentPanel.Controls.Add(table);

            Label ownerLabel = new Label
            {
                Text = "Propriétaire:",
                AutoSize = true,
                Anchor = AnchorStyles.Left | AnchorStyles.Right
            };
            table.Controls.Add(ownerLabel, 0, 0);

            _ownerSelect = new ComboBox
            {
                Width = 200,
                Anchor = AnchorStyles.Left | AnchorStyles.Right
            };
            List<Owner> owners = MarketCSharp.tool.Owner.GetOwners(_conn);
            foreach (Owner owner in owners)
            {
                _ownerSelect.Items.Add($"{owner.GetName()} {owner.GetFirstName()}");
            }
            if (_ownerSelect.Items.Count > 0)
                _ownerSelect.SelectedIndex = 0;
            table.Controls.Add(_ownerSelect, 1, 0);

            Label dateLabel = new Label
            {
                Text = "Date de paiement:",
                AutoSize = true,
                Anchor = AnchorStyles.Left | AnchorStyles.Right
            };
            table.Controls.Add(dateLabel, 0, 1);

            _dateEntry = new DateTimePicker
            {
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "yyyy-MM-dd",
                Width = 200,
                Anchor = AnchorStyles.Left | AnchorStyles.Right
            };
            table.Controls.Add(_dateEntry, 1, 1);

            Label amountLabel = new Label
            {
                Text = "Montant:",
                AutoSize = true,
                Anchor = AnchorStyles.Left | AnchorStyles.Right
            };
            table.Controls.Add(amountLabel, 0, 2);

            _amountEntry = new TextBox
            {
                Width = 200,
                Anchor = AnchorStyles.Left | AnchorStyles.Right
            };
            table.Controls.Add(_amountEntry, 1, 2);

            Button paymentButton = new Button
            {
                Text = "Effectuer le paiement",
                BackColor = System.Drawing.ColorTranslator.FromHtml("#5A9"),
                ForeColor = System.Drawing.Color.Black,
                Font = new System.Drawing.Font("Arial", 12),
                FlatStyle = FlatStyle.Flat,
                Width = 200,
                Anchor = AnchorStyles.None
            };
            paymentButton.Click += new EventHandler(ProcessPayment);
            table.Controls.Add(paymentButton, 1, 3);
        }

        private void InitInsertMarketPage()
        {
            // Nettoyer le panel
            _insertMarketPanel.Controls.Clear();
            
            // Layout principal avec padding
            _insertMarketPanel.Padding = new Padding(20);
            
            Label insertMarketLabel = new Label
            {
                Text = "Insérer un marché",
                Font = new System.Drawing.Font("Arial", 16, System.Drawing.FontStyle.Bold),
                AutoSize = true,
                Location = new System.Drawing.Point(20, 20)
            };
            _insertMarketPanel.Controls.Add(insertMarketLabel);

            // Tableau pour organiser les contrôles
            TableLayoutPanel table = new TableLayoutPanel
            {
                Location = new System.Drawing.Point(20, 70),
                ColumnCount = 2,
                RowCount = 6,
                AutoSize = true,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None
            };
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));
            _insertMarketPanel.Controls.Add(table);

            Label marketLongLabel = new Label
            {
                Text = "Longueur:",
                AutoSize = true,
                Anchor = AnchorStyles.Left | AnchorStyles.Right
            };
            table.Controls.Add(marketLongLabel, 0, 0);

            _marketLongEntry = new TextBox
            {
                Width = 200,
                Anchor = AnchorStyles.Left | AnchorStyles.Right
            };
            table.Controls.Add(_marketLongEntry, 1, 0);

            Label marketLargLabel = new Label
            {
                Text = "Largeur:",
                AutoSize = true,
                Anchor = AnchorStyles.Left | AnchorStyles.Right
            };
            table.Controls.Add(marketLargLabel, 0, 1);

            _marketLargEntry = new TextBox
            {
                Width = 200,
                Anchor = AnchorStyles.Left | AnchorStyles.Right
            };
            table.Controls.Add(_marketLargEntry, 1, 1);

            Label marketXLabel = new Label
            {
                Text = "X:",
                AutoSize = true,
                Anchor = AnchorStyles.Left | AnchorStyles.Right
            };
            table.Controls.Add(marketXLabel, 0, 2);

            _marketXEntry = new TextBox
            {
                Width = 200,
                Anchor = AnchorStyles.Left | AnchorStyles.Right
            };
            table.Controls.Add(_marketXEntry, 1, 2);

            Label marketYLabel = new Label
            {
                Text = "Y:",
                AutoSize = true,
                Anchor = AnchorStyles.Left | AnchorStyles.Right
            };
            table.Controls.Add(marketYLabel, 0, 3);

            _marketYEntry = new TextBox
            {
                Width = 200,
                Anchor = AnchorStyles.Left | AnchorStyles.Right
            };
            table.Controls.Add(_marketYEntry, 1, 3);

            Label marketNomMarketLabel = new Label
            {
                Text = "Nom market:",
                AutoSize = true,
                Anchor = AnchorStyles.Left | AnchorStyles.Right
            };
            table.Controls.Add(marketNomMarketLabel, 0, 4);

            _marketNomMarketEntry = new TextBox
            {
                Width = 200,
                Anchor = AnchorStyles.Left | AnchorStyles.Right
            };
            table.Controls.Add(_marketNomMarketEntry, 1, 4);

            Button insertMarketButton = new Button
            {
                Text = "Insérer Market",
                BackColor = System.Drawing.ColorTranslator.FromHtml("#5A9"),
                ForeColor = System.Drawing.Color.Black,
                Font = new System.Drawing.Font("Arial", 12),
                FlatStyle = FlatStyle.Flat,
                Width = 200,
                Anchor = AnchorStyles.None
            };
            insertMarketButton.Click += new EventHandler(InsertMarket);
            table.Controls.Add(insertMarketButton, 1, 5);
        }

        private void InitInsertBoxPage()
        {
            // Nettoyer le panel
            _insertBoxPanel.Controls.Clear();
            
            // Layout principal avec padding
            _insertBoxPanel.Padding = new Padding(20);
            
            Label insertBoxLabel = new Label
            {
                Text = "Insérer un box",
                Font = new System.Drawing.Font("Arial", 16, System.Drawing.FontStyle.Bold),
                AutoSize = true,
                Location = new System.Drawing.Point(20, 20)
            };
            _insertBoxPanel.Controls.Add(insertBoxLabel);

            // Tableau pour organiser les contrôles
            TableLayoutPanel table = new TableLayoutPanel
            {
                Location = new System.Drawing.Point(20, 70),
                ColumnCount = 2,
                RowCount = 7,
                AutoSize = true,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None
            };
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));
            _insertBoxPanel.Controls.Add(table);

            Label idMarketLabel = new Label
            {
                Text = "ID Market:",
                AutoSize = true,
                Anchor = AnchorStyles.Left | AnchorStyles.Right
            };
            table.Controls.Add(idMarketLabel, 0, 0);

            _idMarketSelect = new ComboBox
            {
                Width = 200,
                Anchor = AnchorStyles.Left | AnchorStyles.Right
            };
            List<Market> markets = Market.GetMarkets(_conn);
            foreach (Market market in markets)
            {
                _idMarketSelect.Items.Add(market.GetIdMarket());
            }
            if (_idMarketSelect.Items.Count > 0)
                _idMarketSelect.SelectedIndex = 0;
            table.Controls.Add(_idMarketSelect, 1, 0);

            Label boxNumLabel = new Label
            {
                Text = "Numéro:",
                AutoSize = true,
                Anchor = AnchorStyles.Left | AnchorStyles.Right
            };
            table.Controls.Add(boxNumLabel, 0, 1);

            _boxNumEntry = new TextBox
            {
                Width = 200,
                Anchor = AnchorStyles.Left | AnchorStyles.Right
            };
            table.Controls.Add(_boxNumEntry, 1, 1);

            Label boxLongLabel = new Label
            {
                Text = "Longueur:",
                AutoSize = true,
                Anchor = AnchorStyles.Left | AnchorStyles.Right
            };
            table.Controls.Add(boxLongLabel, 0, 2);

            _boxLongEntry = new TextBox
            {
                Width = 200,
                Anchor = AnchorStyles.Left | AnchorStyles.Right
            };
            table.Controls.Add(_boxLongEntry, 1, 2);

            Label boxLargLabel = new Label
            {
                Text = "Largeur:",
                AutoSize = true,
                Anchor = AnchorStyles.Left | AnchorStyles.Right
            };
            table.Controls.Add(boxLargLabel, 0, 3);

            _boxLargEntry = new TextBox
            {
                Width = 200,
                Anchor = AnchorStyles.Left | AnchorStyles.Right
            };
            table.Controls.Add(_boxLargEntry, 1, 3);

            Label boxXLabel = new Label
            {
                Text = "X:",
                AutoSize = true,
                Anchor = AnchorStyles.Left | AnchorStyles.Right
            };
            table.Controls.Add(boxXLabel, 0, 4);

            _boxXEntry = new TextBox
            {
                Width = 200,
                Anchor = AnchorStyles.Left | AnchorStyles.Right
            };
            table.Controls.Add(_boxXEntry, 1, 4);

            Label boxYLabel = new Label
            {
                Text = "Y:",
                AutoSize = true,
                Anchor = AnchorStyles.Left | AnchorStyles.Right
            };
            table.Controls.Add(boxYLabel, 0, 5);

            _boxYEntry = new TextBox
            {
                Width = 200,
                Anchor = AnchorStyles.Left | AnchorStyles.Right
            };
            table.Controls.Add(_boxYEntry, 1, 5);

            Button insertBoxButton = new Button
            {
                Text = "Insérer Box",
                BackColor = System.Drawing.ColorTranslator.FromHtml("#5A9"),
                ForeColor = System.Drawing.Color.Black,
                Font = new System.Drawing.Font("Arial", 12),
                FlatStyle = FlatStyle.Flat,
                Width = 200,
                Anchor = AnchorStyles.None
            };
            insertBoxButton.Click += new EventHandler(InsertBox);
            table.Controls.Add(insertBoxButton, 1, 6);
        }

        private void ValidateDate(object sender, EventArgs e)
        {
            string yearMonth = _yearMonthTextBox.Text;
            
            // Vérifier que la date est au format YYYY-MM
            if (yearMonth.Length == 7 && yearMonth[4] == '-')
            {
                if (!_yearMonthComboBox.Items.Contains(yearMonth))
                {
                    _yearMonthComboBox.Items.Add(yearMonth);
                }
                _yearMonthComboBox.SelectedItem = yearMonth;
                _canvasPanel.Controls.Clear();
                DisplayMarkets();
            }
            else
            {
                MessageBox.Show("Format de date invalide. Utilisez le format YYYY-MM (ex: 2022-01)", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DisplayMarkets()
        {
            DisplayLegends();
            
            if (_markets != null && _markets.Count > 0)
            {
                foreach (Market market in _markets)
                {
                    DisplayMarket(market);
                }
            }
            else
            {
                Label noDataLabel = new Label
                {
                    Text = "Aucun marché à afficher",
                    AutoSize = true,
                    Location = new System.Drawing.Point(50, 100),
                    Font = new System.Drawing.Font("Arial", 12)
                };
                _canvasPanel.Controls.Add(noDataLabel);
            }
        }

        private void DisplayLegends()
        {
            Panel legendPanel = new Panel
            {
                Location = new System.Drawing.Point(10, 10),
                Size = new System.Drawing.Size(750, 30),
                BorderStyle = BorderStyle.FixedSingle
            };
            _canvasPanel.Controls.Add(legendPanel);

            int legendSpacing = 200;
            
            // Légende pour le pourcentage payé
            Panel paidSample = new Panel
            {
                Size = new System.Drawing.Size(20, 20),
                Location = new System.Drawing.Point(10, 5),
                BackColor = System.Drawing.ColorTranslator.FromHtml("#4A8"),
                BorderStyle = BorderStyle.FixedSingle
            };
            legendPanel.Controls.Add(paidSample);
            
            Label legendPaid = new Label
            {
                Text = "Pourcentage payé",
                Location = new System.Drawing.Point(35, 8),
                AutoSize = true
            };
            legendPanel.Controls.Add(legendPaid);
            
            // Légende pour le pourcentage non payé
            Panel unpaidSample = new Panel
            {
                Size = new System.Drawing.Size(20, 20),
                Location = new System.Drawing.Point(10 + legendSpacing, 5),
                BackColor = System.Drawing.ColorTranslator.FromHtml("#D9534F"),
                BorderStyle = BorderStyle.FixedSingle
            };
            legendPanel.Controls.Add(unpaidSample);
            
            Label legendUnpaid = new Label
            {
                Text = "Pourcentage non payé",
                Location = new System.Drawing.Point(35 + legendSpacing, 8),
                AutoSize = true
            };
            legendPanel.Controls.Add(legendUnpaid);
            
            // Légende pour non loué
            Panel notRentedSample = new Panel
            {
                Size = new System.Drawing.Size(20, 20),
                Location = new System.Drawing.Point(10 + 2 * legendSpacing, 5),
                BackColor = System.Drawing.ColorTranslator.FromHtml("#DAA520"),
                BorderStyle = BorderStyle.FixedSingle
            };
            legendPanel.Controls.Add(notRentedSample);
            
            Label legendNotRented = new Label
            {
                Text = "Non loué",
                Location = new System.Drawing.Point(35 + 2 * legendSpacing, 8),
                AutoSize = true
            };
            legendPanel.Controls.Add(legendNotRented);
        }

        private void DisplayMarket(Market market)
        {
            if (_yearMonthComboBox.SelectedItem == null)
            {
                MessageBox.Show("Veuillez sélectionner une année et un mois.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Récupérer et afficher les boxes pour ce marché
            List<Box> boxes = market.GetBoxs(_conn);
            if (boxes != null && boxes.Count > 0)
            {
                foreach (Box box in boxes)
                {
                    DisplayBox(box);
                }
            }

            int x = (int)(market.GetX() * 20) + 50; // Ajouter un offset pour éviter les bords
            int y = (int)(market.GetY() * 20) + 50; // Ajouter un offset pour éviter les bords
            int longueur = (int)(market.GetLongueur() * 10);
            int largeur = (int)(market.GetLargeur() * 10);

            // Créer un panel pour représenter le marché
            Panel marketPanel = new Panel
            {
                Location = new System.Drawing.Point(x, y),
                Size = new System.Drawing.Size(longueur, largeur),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.Transparent
            };
            _canvasPanel.Controls.Add(marketPanel);

            // Ajouter le nom du marché
            Label marketLabel = new Label
            {
                Text = market.GetNomMarket(),
                Location = new System.Drawing.Point(x + longueur / 2 - 40, y - 20),
                AutoSize = true,
                Font = new System.Drawing.Font("Helvetica", 10, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.ColorTranslator.FromHtml("#5A9")
            };
            _canvasPanel.Controls.Add(marketLabel);
        }

        

        private void DisplayBox(Box box)
        {
            int x = (int)(box.GetX() * 15);
            int y = (int)(box.GetY() * 20);
            int longueur = (int)(box.GetLongueur() * 10);
            int largeur = (int)(box.GetLargeur() * 10);
            string yearMonth = _yearMonthComboBox.SelectedItem.ToString();
            double percentPaid = box.GetPourcent(_conn, yearMonth);
            int greenWidth = (int)(longueur * percentPaid);
            int redWidth = longueur - greenWidth;
            Label boxLabel = new Label
            {
                Text = $"Num : {box.GetNum()}",
                BackColor = Color.Transparent,
                Location = new System.Drawing.Point(x , y + largeur),
                AutoSize = true,
                Font = new System.Drawing.Font("Helvetica", 8, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.Black
            };
            _canvasPanel.Controls.Add(boxLabel);

            Panel boxPanel = new Panel
            {
                Location = new System.Drawing.Point(x, y),
                Size = new System.Drawing.Size(longueur, largeur),
                BorderStyle = BorderStyle.FixedSingle
            };

            if (!box.IsBoxRented(_conn, yearMonth))
            {
                boxPanel.BackColor = System.Drawing.ColorTranslator.FromHtml("#DAA520");
            }
            else
            {
                Location location = MarketCSharp.tool.Location.GetLocationByBoxAndYearMonth(_conn, box.GetIdBox(), yearMonth);
                if (location != null)
                {
                    Owner owner = MarketCSharp.tool.Owner.GetOwnerById(_conn, location.GetIdOwner());
                    if (owner != null)
                    {
                        Label ownerLabel = new Label
                        {
                            Text = $"Owner: {owner.GetName()}",
                            Location = new System.Drawing.Point(x + longueur / 10, y - largeur / 2),
                            AutoSize = true,
                            Font = new System.Drawing.Font("Helvetica", 5, System.Drawing.FontStyle.Bold),
                            ForeColor = System.Drawing.Color.Black,
                            BackColor = Color.Transparent
                        };
                        _canvasPanel.Controls.Add(ownerLabel);
                    }
                }

                if (greenWidth > 0)
                {
                    Panel greenPanel = new Panel
                    {
                        Location = new System.Drawing.Point(x, y),
                        Size = new System.Drawing.Size(greenWidth, largeur),
                        BorderStyle = BorderStyle.FixedSingle,
                        BackColor = System.Drawing.ColorTranslator.FromHtml("#4A8")
                    };
                    _canvasPanel.Controls.Add(greenPanel);
                }
                if (redWidth > 0)
                {
                    Panel redPanel = new Panel
                    {
                        Location = new System.Drawing.Point(x + greenWidth, y),
                        Size = new System.Drawing.Size(redWidth, largeur),
                        BorderStyle = BorderStyle.FixedSingle,
                        BackColor = System.Drawing.ColorTranslator.FromHtml("#D9534F")
                    };
                    _canvasPanel.Controls.Add(redPanel);
                }
            }
        }

        private Dictionary<int, double> CalculateAllDebts(string yearMonth)
        {
            try
            {
                List<Owner> owners = MarketCSharp.tool.Owner.GetOwners(_conn);
                Console.WriteLine(owners);
                Dictionary<int, double> debts = new Dictionary<int, double>();
                foreach (Owner owner in owners)
                {
                    List<Box> ownerBoxes = MarketCSharp.material.Box.GetBoxByIdOwner(_conn, owner.GetIdOwner());
                    List<Location> ownerLocations = MarketCSharp.tool.Location.GetLocationByOwner(_conn, owner.GetIdOwner());

                    if (ownerBoxes.Count == 0)
                    {
                        debts[owner.GetIdOwner()] = 0;
                        continue;
                    }

                    double totalDebt = 0;
                    DateTime yearMonthDate = DateTime.ParseExact(yearMonth, "yyyy-MM", null);

                    foreach (Location location in ownerLocations)
                    {
                        Box box = Box.GetBoxById(_conn, location.GetIdBox());
                        if (box != null)
                        {
                            DateTime currentDate = location.GetDebut();
                            while (currentDate <= yearMonthDate && (location.GetFin() == null || currentDate <= location.GetFin()))
                            {
                                string yearMonthCurrent = currentDate.ToString("yyyy-MM");
                                double rent = box.CalculRent(_conn, yearMonthCurrent);
                                Paiement paiement = Paiement.GetPaiement(_conn, yearMonthCurrent, box.GetIdBox());
                                if (paiement != null)
                                {
                                    double remainingRent = rent - paiement.GetMontant();
                                    if (remainingRent > 0)
                                    {
                                        totalDebt += remainingRent;
                                    }
                                }
                                else
                                {
                                    totalDebt += rent;
                                }
                                currentDate = currentDate.AddMonths(1);
                            }
                        }
                    }

                    debts[owner.GetIdOwner()] = totalDebt;
                }

                return debts;
            }
            catch (Exception e)
            {
                MessageBox.Show($"Error calculating debts: {e.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new Dictionary<int, double>();
            }
        }

        private void InsertMarket(object sender, EventArgs e)
        {
            try
            {
                double longueur = double.Parse(_marketLongEntry.Text);
                double largeur = double.Parse(_marketLargEntry.Text);
                int x = int.Parse(_marketXEntry.Text);
                int y = int.Parse(_marketYEntry.Text);
                string nomMarket = _marketNomMarketEntry.Text;

                if (string.IsNullOrEmpty(nomMarket))
                {
                    throw new Exception("Nom market is required.");
                }

                Market.InsertMarket(_conn, longueur, largeur, x, y, nomMarket);
                MessageBox.Show("Market inserted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (FormatException)
            {
                MessageBox.Show("Please enter valid numeric values for longueur, largeur, x, and y.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error inserting market: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InsertBox(object sender, EventArgs e)
        {
            try
            {
                int idMarket = int.Parse(_idMarketSelect.SelectedItem.ToString());
                int num = int.Parse(_boxNumEntry.Text);
                double longueur = double.Parse(_boxLongEntry.Text);
                double largeur = double.Parse(_boxLargEntry.Text);
                int x = int.Parse(_boxXEntry.Text);
                int y = int.Parse(_boxYEntry.Text);

                Box.InsertBox(_conn, idMarket, num, longueur, largeur, x, y);
                MessageBox.Show("Box inserted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (FormatException)
            {
                MessageBox.Show("Please enter valid numeric values for ID Market, Numéro, Longueur, Largeur, X, and Y.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error inserting box: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ProcessPayment(object sender, EventArgs e)
        {
            string ownerName = _ownerSelect.SelectedItem.ToString();
            string datePaiement = _dateEntry.Value.ToString("yyyy-MM-dd");
            double montant = double.Parse(_amountEntry.Text);

            try
            {
                Owner owner = MarketCSharp.tool.Owner.GetOwners(_conn).Find(o => $"{o.GetName()} {o.GetFirstName()}" == ownerName);
                List<Box> ownerBoxes = Box.GetBoxByIdOwner(_conn, owner.GetIdOwner());
                List<Location> ownerLocations = MarketCSharp.tool.Location.GetLocationByOwner(_conn, owner.GetIdOwner());

                if (ownerBoxes.Count == 0)
                {
                    MessageBox.Show("Cet owner n'a pas de boxs.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                List<DateTime> startDates = ownerLocations.ConvertAll(location => location.GetDebut());
                startDates.Sort();
                DateTime currentDate = startDates[0];
                bool hasNoEndDate = ownerLocations.Exists(location => location.GetFin() == null);
                DateTime? latestEndDate = null;

                if (!hasNoEndDate)
                {
                    List<DateTime> endDates = ownerLocations.FindAll(location => location.GetFin() != null).ConvertAll(location => location.GetFin().Value);
                    endDates.Sort((a, b) => b.CompareTo(a));
                    latestEndDate = endDates.Count > 0 ? endDates[0] : (DateTime?)null;
                }

                ownerLocations.Sort((loc1, loc2) => Box.GetBoxById(_conn, loc1.GetIdBox()).GetNum().CompareTo(Box.GetBoxById(_conn, loc2.GetIdBox()).GetNum()));

                while (montant > 0)
                {
                    foreach (Location location in ownerLocations)
                    {
                        Box box = Box.GetBoxById(_conn, location.GetIdBox());
                        if (box != null)
                        {
                            DateTime yearMonth = currentDate;
                            if (location.GetDebut() <= yearMonth && (location.GetFin() == null || yearMonth <= location.GetFin().Value))
                            {
                                DateTime? finLocation = location.GetFin();
                                montant = box.InsertPaiement(_conn, DateTime.Parse(datePaiement), montant, yearMonth.ToString("yyyy-MM"), finLocation);
                            }
                        }
                    }

                    currentDate = currentDate.AddMonths(1);
                    if (latestEndDate.HasValue && currentDate > latestEndDate.Value)
                    {
                        break;
                    }
                }

                if (montant > 0)
                {
                    MessageBox.Show($"Payment processed successfully. Remaining amount: {montant}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Payment processed successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error processing payment: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}