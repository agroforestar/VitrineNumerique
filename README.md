# Vitrine Numérique

Ce projet Unity propose deux fonctionnalités. La première permet de projeter en taille réelle des parcelles pré-faites et de modifier la taille et le type d'arbres. La deuxième fonctionnalité permet, à partir d'une parcelle vierge, de positionner des allées d'arbres ou des haies, en tenant compte d'un espacement choisi.

## Installation

1. **Prérequis**
   Assurez-vous d'avoir Unity installé sur votre système.

2. **Téléchargement et Configuration**
   - Clonez le dépôt du projet depuis GitHub.
   - Ouvrez Unity Hub et ajoutez le projet Unity téléchargé.

3. **Lancement**
   - Ouvrez le projet dans Unity.
   - Build le projet sur un appareil avec Android 7 au minimum.

## Fonctionnalités

### Scripts Principaux

1. **ARScene.cs**
   - Ce script gère la scène de réalité augmentée (AR) et l'interaction avec les plantes.
   - Il contrôle la croissance des plantes, le changement de saison et l'affichage du système jour/nuit.

2. **MainMenu.cs**
   - Gère le menu principal, permettant de naviguer vers la fonction "parcelles pré-faites" ou "création de parcelles".

3. **Menu.cs**
   - Contrôle le menu d'options et le choix du système dans le mode "parcelles pré-faites".

4. **PosInterpolation.cs**
   - Gère l'interpolation de la position des arbres ou des haies entre deux points GPS.
   - Prend en compte l'espacement spécifié et permet de placer les éléments de manière régulière.

### Utilisation

- Au démarrage, l'application affiche un menu permettant de choisir entre le mode "parcelles pré-faites" et "création de parcelles".
- Dans le mode "création de parcelles", il faut utiliser le bouton "Enregistrer la position GPS" en bas à droite de l'écran pour marquer sa position GPS. Une fois une deuxième position GPS enregistrée, une rangée de plantes apparaîtra entre les points.  
- Il est nécessaire de s'éloigner d'au moins 5 mètres environ entre les deux points GPS. Si les deux points sont trop proches, un message s'affichera et une seule plante sera créée.

### Notes Additionnelles

- L'application utilise la réalité augmentée (AR). ARCore doit être à jour sur le mobile ou la tablette utilisée.
- L'application utilise la géolocalisation pour déterminer la position des éléments.
- Les plantes sont placées en fonction des points GPS récupérés.

## Packages nécessaires

- AR Foundation 4.2.7
- ARCore XR Plugin 4.2.7
- ARKit XR Plugin 4.2.7
- JetBrains Rider Editor 3.0.18
- Test Framework 1.1.31
- TextMeshPro 3.0.6 
- Timeline 1.6.4
- Tutorial Framework 3.1.1 
- Unity UI 1.0.0
- Universal RP 12.1.10
- Version Control 2.0.1
- Visual Scripting 1.8.0
- Visual Studio Code Editor 1.2.5
- Visual Studio Editor 2.0.17 
- XR Plugin Management 4.2.0 

## Contributeurs

- Clément Dahan
