using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Lab06
{

    public class Specialite
    {
        public string Identifiant { get; set; }
        public string Nom { get; set; }
        public string Description { get; set; }
    }

    // Définition de la classe Medecin
    public class Medecin
    {
        public string Identifiant { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Specialite { get; set; }
        public decimal Salaire { get; set; }
    }

    public partial class MainWindow : Window
    {
        // Listes pour stocker les spécialités et les médecins
        private List<Specialite> specialites = new List<Specialite>();
        private List<Medecin> medecins = new List<Medecin>();

        public MainWindow()
        {
            InitializeComponent();
        }

        // Méthode pour afficher un message dans la StatusBar
        private void AfficherMessage(string message)
        {
            statusMessage.Text = message;
        }


        // Méthode pour ajouter une nouvelle spécialité
        private void AjouterSpecialite(object sender, RoutedEventArgs e)
        {
            // Vérifier si les champs requis sont remplis
            if (string.IsNullOrWhiteSpace(tbIdentifiantSpecialite.Text) || string.IsNullOrWhiteSpace(tbNomSpecialite.Text) || string.IsNullOrWhiteSpace(tbDescriptionSpecialitex.Text))
            {
                AfficherMessage("Veuillez remplir tous les champs.");
                return;
            }

            // Vérifier si l'identifiant de la spécialité est unique
            if (specialites.Any(s => s.Identifiant == tbIdentifiantSpecialite.Text))
            {
                AfficherMessage("Une spécialité avec cet identifiant existe déjà.");
                return;
            }

            // Vérifier si le nom de la spécialité est unique
            if (specialites.Any(s => s.Nom == tbNomSpecialite.Text))
            {
                AfficherMessage("Une spécialité avec ce nom existe déjà.");
                return;
            }

            // Créer une nouvelle spécialité
            Specialite nouvelleSpecialite = new Specialite
            {
                Identifiant = tbIdentifiantSpecialite.Text,
                Nom = tbNomSpecialite.Text,
                Description = tbDescriptionSpecialitex.Text
            };

            // Ajouter la spécialité à la liste
            specialites.Add(nouvelleSpecialite);

            // Mettre à jour la DataGrid
            dgSpecialites.ItemsSource = null; // Réinitialiser la source de données
            dgSpecialites.ItemsSource = specialites;

            // Mettre à jour la liste des spécialités dans le ComboBox tbDetailProgrammex
            tbDetailProgrammex.ItemsSource = specialites.Select(s => s.Nom).ToList();

            // Mettre à jour la liste des spécialités dans le ComboBox tbNomSpecialitex
            tbNomSpecialitex.ItemsSource = specialites.Select(s => s.Nom).ToList();

            // Réinitialiser les champs de saisie
            tbIdentifiantSpecialite.Text = string.Empty;
            tbNomSpecialite.Text = string.Empty;
            tbDescriptionSpecialitex.Text = string.Empty;

            // Afficher un message de réussite
            AfficherMessage("Spécialité ajoutée avec succès.");
        }


        // Méthode pour ajouter un médecin
        private void AjouterMedecin(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbIDProgramme.Text) || string.IsNullOrWhiteSpace(tbTitreProgramme.Text) ||
    string.IsNullOrWhiteSpace(tbDetailProgramme.Text) || string.IsNullOrWhiteSpace(tbSalaire.Text) ||
    tbDetailProgrammex.SelectedItem == null)
            {
                AfficherMessage("Veuillez remplir tous les champs.");
                return;
            }

            // Déclarer une variable pour stocker le salaire
            int salaire;

            // Essayer de convertir la chaîne de salaire en nombre entier
            if (!int.TryParse(tbSalaire.Text, out salaire))
            {
                // Afficher un message d'erreur à l'utilisateur
                AfficherMessage("Le salaire doit être un nombre entier.");
                return; // Sortir de la méthode sans créer un nouveau médecin
            }

            // Créer un nouveau médecin avec le salaire converti
            Medecin nouveauMedecin = new Medecin
            {
                Identifiant = tbIDProgramme.Text, // Utiliser l'identifiant saisi
                Nom = tbTitreProgramme.Text,
                Prenom = tbDetailProgramme.Text,
                Specialite = (string)tbDetailProgrammex.SelectedItem,
                Salaire = salaire
            };

            // Ajouter le médecin à la liste
            medecins.Add(nouveauMedecin);

            // Mettre à jour la source de données de la DataGrid
            dgProgramme.ItemsSource = null;
            dgProgramme.ItemsSource = medecins;

            // Réinitialiser les champs du formulaire
            tbTitreProgramme.Text = string.Empty;
            tbDetailProgramme.Text = string.Empty;
            tbDetailProgrammex.SelectedItem = null; // Effacer la sélection dans le ComboBox
            tbSalaire.Text = string.Empty;
            tbIDProgramme.Text = string.Empty; // Réinitialiser le champ de l'identifiant

            // Afficher un message de réussite
            AfficherMessage("Médecin ajouté avec succès.");
        }

        // Méthode pour supprimer une spécialité
        private void SupprimerSpecialite(object sender, RoutedEventArgs e)
        {
            // Vérifier si une spécialité est sélectionnée dans la DataGrid
            if (dgSpecialites.SelectedItem == null)
            {
                AfficherMessage("Veuillez sélectionner une spécialité à supprimer.");
                return;
            }

            // Récupérer la spécialité sélectionnée dans la DataGrid
            Specialite specialiteASupprimer = (Specialite)dgSpecialites.SelectedItem;

            // Vérifier si des médecins exercent cette spécialité
            if (medecins.Any(m => m.Specialite == specialiteASupprimer.Nom))
            {
                AfficherMessage("Impossible de supprimer cette spécialité car elle est associée à des médecins.");
                return;
            }

            // Supprimer la spécialité de la liste
            specialites.Remove(specialiteASupprimer);

            // Mettre à jour la source de la DataGrid
            dgSpecialites.ItemsSource = null;
            dgSpecialites.ItemsSource = specialites;

            // Afficher un message de réussite
            AfficherMessage("Spécialité supprimée avec succès.");
        }

        
        // Méthode pour supprimer un médecin
        private void SupprimerMedecin(object sender, RoutedEventArgs e)
        {
            // Vérifier si un médecin est sélectionné
            if (dgProgramme.SelectedItem == null)
            {
                AfficherMessage("Veuillez sélectionner un médecin à supprimer.");
                return;
            }

            // Supprimer le médecin de la liste
            Medecin medecinASupprimer = (Medecin)dgProgramme.SelectedItem;
            medecins.Remove(medecinASupprimer);

            // Mettre à jour la source de données de la DataGrid
            dgProgramme.ItemsSource = null;
            dgProgramme.ItemsSource = medecins;

            // Afficher un message de réussite
            AfficherMessage("Médecin supprimé avec succès.");
        }

        // Méthode pour modifier un médecin existant
        private void ModifierMedecin(object sender, RoutedEventArgs e)
        {
            // Vérifier si un médecin est sélectionné
            if (dgProgramme.SelectedItem == null)
            {
                AfficherMessage("Veuillez sélectionner un médecin à modifier.");
                return;
            }

            // Obtenir le médecin sélectionné dans la DataGrid
            Medecin medecinAModifier = (Medecin)dgProgramme.SelectedItem;

            // Vérifier si les champs requis sont remplis
            if (string.IsNullOrWhiteSpace(tbTitreProgramme.Text) || string.IsNullOrWhiteSpace(tbDetailProgramme.Text) ||
                string.IsNullOrWhiteSpace(tbSalaire.Text) || tbDetailProgrammex.SelectedItem == null)
            {
                AfficherMessage("Veuillez remplir tous les champs.");
                return;
            }

            // Déclarer une variable pour stocker le salaire
            int salaire;

            // Essayer de convertir la chaîne de salaire en nombre entier
            if (!int.TryParse(tbSalaire.Text, out salaire))
            {
                // Afficher un message d'erreur à l'utilisateur
                AfficherMessage("Le salaire doit être un nombre entier.");
                return; // Sortir de la méthode sans modifier le médecin
            }

            // Mettre à jour les propriétés du médecin avec les nouvelles valeurs des champs
            medecinAModifier.Nom = tbTitreProgramme.Text;
            medecinAModifier.Prenom = tbDetailProgramme.Text;
            medecinAModifier.Specialite = (string)tbDetailProgrammex.SelectedItem;
            medecinAModifier.Salaire = salaire;

            // Mettre à jour la source de données de la DataGrid
            dgProgramme.Items.Refresh();

            // Afficher un message de réussite
            AfficherMessage("Médecin modifié avec succès.");
        }


        private void tbNomSpecialitex_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Récupérer la spécialité sélectionnée dans le ComboBox
            string specialiteSelectionnee = (string)tbNomSpecialitex.SelectedItem;

            // Filtrer la liste des médecins pour n'inclure que ceux avec la spécialité sélectionnée
            var medecinsSpecialite = medecins.Where(m => m.Specialite == specialiteSelectionnee).ToList();

            // Mettre à jour la source de la DataGrid avec la liste filtrée de médecins
            dgProgramme.ItemsSource = medecinsSpecialite;
        }

        private void ConsulterSpecialites_Click(object sender, RoutedEventArgs e)
        {
            // Récupérer la spécialité sélectionnée dans le ComboBox
            string specialiteSelectionnee = tbNomSpecialitex.SelectedItem as string;

            if (specialiteSelectionnee != null)
            {
                // Filtrer les médecins par la spécialité sélectionnée
                var medecinsPourSpecialite = medecins.Where(m => m.Specialite == specialiteSelectionnee).ToList();

                // Mettre à jour la DataGrid avec les médecins filtrés
                dgProgrammeX.ItemsSource = medecinsPourSpecialite;

                // Afficher un message dans la StatusBar
                AfficherMessage($"Liste des médecins pour la spécialité '{specialiteSelectionnee}' affichée.");
            }
            else
            {
                // Afficher un message d'erreur si aucune spécialité n'est sélectionnée
                AfficherMessage("Veuillez sélectionner une spécialité.");
            }
        }

        // Méthode pour modifier une spécialité
        private void ModifierSpecialite(object sender, RoutedEventArgs e)
        {
            if (dgSpecialites.SelectedItem == null)
            {
                AfficherMessage("Veuillez sélectionner une spécialité à modifier.");
                return;
            }

            // Récupérer la spécialité sélectionnée dans la DataGrid
            Specialite specialiteSelectionnee = (Specialite)dgSpecialites.SelectedItem;

            // Afficher les informations de la spécialité sélectionnée dans les champs de saisie
            tbIdentifiantSpecialite.Text = specialiteSelectionnee.Identifiant;
            tbNomSpecialite.Text = specialiteSelectionnee.Nom;
            tbDescriptionSpecialitex.Text = specialiteSelectionnee.Description;

            // Mettre à jour l'état de l'interface utilisateur pour permettre la modification
            // Par exemple, activer ou afficher le bouton "Enregistrer les modifications"
            // et désactiver d'autres boutons qui ne sont pas nécessaires pendant la modification.
        }


        // Méthode pour gérer le clic sur le premier MenuItem
        private void MenuItem_Click_0(object sender, RoutedEventArgs e)
        {
            // Ajoutez ici la logique pour gérer le clic sur le premier MenuItem
        }
        // Méthode pour gérer le clic sur le bouton de fermeture
       


        // Méthode pour quitter l'application
        private void Quitter_Click(object sender, RoutedEventArgs e)
        {
            // Ajoutez ici la logique pour quitter l'application
            Application.Current.Shutdown();
        }

        // Méthode pour gérer le clic sur le deuxième MenuItem
        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            // Ajoutez ici la logique pour gérer le clic sur le deuxième MenuItem
        }

        // Méthode pour gérer le clic sur le troisième MenuItem
        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            // Ajoutez ici la logique pour gérer le clic sur le troisième MenuItem
        }

        // Méthode pour consulter les médecins
        private void ConsulterMedecin(object sender, RoutedEventArgs e)
        {
            // Vérifier si une spécialité est sélectionnée
            if (tbNomSpecialitex.SelectedItem == null)
            {
                AfficherMessage("Veuillez sélectionner une spécialité pour consulter les médecins.");
                return;
            }

            // Récupérer le nom de la spécialité sélectionnée
            string specialiteSelectionnee = tbNomSpecialitex.SelectedItem.ToString();

            // Filtrer la liste des médecins pour la spécialité sélectionnée
            var medecinsParSpecialite = medecins.Where(m => m.Specialite == specialiteSelectionnee).ToList();

            // Mettre à jour la DataGrid avec les médecins filtrés
            dgProgrammeX.ItemsSource = medecinsParSpecialite;

            // Afficher un message dans la StatusBar
            AfficherMessage($"Liste des médecins pour la spécialité '{specialiteSelectionnee}'.");
        }


        // Méthode pour réinitialiser les champs
        private void Nouveau(object sender, RoutedEventArgs e)
        {
            // Réinitialiser les champs de saisie pour la spécialité
            tbNomSpecialite.Text = string.Empty;
            tbDescriptionSpecialitex.Text = string.Empty;
            tbIdentifiantSpecialite.Text = string.Empty;

            // Réinitialiser les champs de saisie pour le médecin
            tbTitreProgramme.Text = string.Empty;
            tbDetailProgramme.Text = string.Empty;
            tbDetailProgrammex.SelectedItem = null;
            tbSalaire.Text = string.Empty;
            tbIDProgramme.Text = string.Empty;

            // Afficher un message de confirmation
            AfficherMessage("Champs réinitialisés.");
        }


        // Méthode pour ajouter une nouvelle entrée
        private void Ajouter(object sender, RoutedEventArgs e)
        {
            AjouterSpecialite(sender, e);

            AjouterMedecin(sender, e);

        }


            // Méthode pour gérer la sélection dans la DataGrid
            private void SelectedProgramme(object sender, RoutedEventArgs e)
        {
            // Vérifier si un médecin est sélectionné dans la DataGrid
            if (dgProgramme.SelectedItem != null)
            {
                // Récupérer le médecin sélectionné
                Medecin medecinSelectionne = (Medecin)dgProgramme.SelectedItem;

                // Remplir les champs de saisie avec les informations du médecin sélectionné
                tbIDProgramme.Text = medecinSelectionne.Identifiant;
                tbTitreProgramme.Text = medecinSelectionne.Nom;
                tbDetailProgramme.Text = medecinSelectionne.Prenom;
                tbDetailProgrammex.Text = medecinSelectionne.Specialite;
                tbSalaire.Text = medecinSelectionne.Salaire.ToString();
            }
        
    }

        // Méthode pour ajouter un nouveau médecin (bouton)
        private void btnAjouterMedecin_Click(object sender, RoutedEventArgs e)
        {
            AjouterMedecin(sender, e);
        }

        private void dgSpecialites_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Vérifier si une spécialité est sélectionnée dans la DataGrid
            if (dgSpecialites.SelectedItem != null)
            {
                // Récupérer la spécialité sélectionnée
                Specialite specialiteSelectionnee = (Specialite)dgSpecialites.SelectedItem;

                // Remplir les champs de saisie avec les informations de la spécialité sélectionnée
                tbIdentifiantSpecialite.Text = specialiteSelectionnee.Identifiant;
                tbNomSpecialite.Text = specialiteSelectionnee.Nom;
                tbDescriptionSpecialitex.Text = specialiteSelectionnee.Description;
            }
        }

        private void ModifierSpecialite_Click(object sender, RoutedEventArgs e)
        {
            // Vérifier si une spécialité est sélectionnée dans la DataGrid
            if (dgSpecialites.SelectedItem != null)
            {
                // Récupérer la spécialité sélectionnée
                Specialite specialiteSelectionnee = (Specialite)dgSpecialites.SelectedItem;

                // Mettre à jour les propriétés de la spécialité avec les valeurs des champs de saisie
                specialiteSelectionnee.Identifiant = tbIdentifiantSpecialite.Text;
                specialiteSelectionnee.Nom = tbNomSpecialite.Text;
                specialiteSelectionnee.Description = tbDescriptionSpecialitex.Text;

                // Rafraîchir la source de données de la DataGrid
                dgSpecialites.Items.Refresh();

                // Afficher un message de réussite
                AfficherMessage("Spécialité modifiée avec succès.");
            }
        }

        // Méthode pour modifier un médecin existant (bouton)
        private void btnModifierMedecin_Click(object sender, RoutedEventArgs e)
        {
            // Vérifier si un médecin est sélectionné dans la DataGrid
            if (dgProgramme.SelectedItem != null)
            {
                // Récupérer le médecin sélectionné
                Medecin medecinSelectionne = (Medecin)dgProgramme.SelectedItem;

                // Mettre à jour les propriétés du médecin avec les valeurs des champs de saisie
                medecinSelectionne.Identifiant = tbIDProgramme.Text;
                medecinSelectionne.Nom = tbTitreProgramme.Text;
                medecinSelectionne.Prenom = tbDetailProgramme.Text;
                medecinSelectionne.Specialite = tbDetailProgrammex.Text;

                // Vérifier si le salaire est un nombre valide
                int salaire;
                if (!int.TryParse(tbSalaire.Text, out salaire))
                {
                    // Afficher un message d'erreur à l'utilisateur
                    AfficherMessage("Veuillez entrer un salaire valide en chiffre .");
                    return; // Sortir de la méthode sans mettre à jour le médecin
                }

                // Mettre à jour le salaire seulement si la conversion est réussie
                medecinSelectionne.Salaire = salaire;

                // Rafraîchir la source de données de la DataGrid
                dgProgramme.Items.Refresh();

                // Afficher un message de réussite
                AfficherMessage("Médecin modifié avec succès.");
            }
        }


        // Méthode pour supprimer un médecin existant (bouton)
        private void btnSupprimerMedecin_Click(object sender, RoutedEventArgs e)
        {
            SupprimerMedecin(sender, e);
        }

       
    }
    
}
