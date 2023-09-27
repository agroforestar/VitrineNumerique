using ARLocation;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Android;
using TMPro;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System;

public class PosInterpolation : MonoBehaviour
{
   

    public float espacement = 7f;
    public TMP_InputField inputEspacement;
   
    private string ArbreOuHaie = "arbre";
    
    private GameObject oldGo;
    
    public static int systemChoose = 0;
   

   

    private GameObject _panelEspacementValid;
    private GameObject _panelEspacementNonValid;

   
    private GameObject _panelTropCourt;
    private GameObject _panelNewpointGPS;
    private GameObject _panelArbresSpawn;
    public TextMeshProUGUI textMeshProArbresSpawn;
    private GameObject _panelDistancentreLesDeuxPointsGPS;
    public TextMeshProUGUI textMeshDistanceMetres;

    public TextMeshProUGUI textMeshUnSeulCree;




    public GameObject treePrefab;
    public GameObject haiePrefab;



    private List<Dictionary<string, float>> _positions = new List<Dictionary<string, float>>();
    [SerializeField] private GameObject wall;
    [SerializeField] private GameObject wall2;

    private void Start()
    {
        
        _panelEspacementValid = GameObject.Find("espacementValid");
        _panelEspacementValid.SetActive(false);

        _panelEspacementNonValid = GameObject.Find("espacementNonValid");
        _panelEspacementNonValid.SetActive(false);
        


        
        _panelTropCourt = GameObject.Find("tropcourt");
        _panelTropCourt.SetActive(false);

        _panelNewpointGPS = GameObject.Find("newpointGPS");
        _panelNewpointGPS.SetActive(false);

        _panelArbresSpawn = GameObject.Find("arbresSpawn");
        _panelArbresSpawn.SetActive(false);

        _panelDistancentreLesDeuxPointsGPS = GameObject.Find("distanceM");
        _panelDistancentreLesDeuxPointsGPS.SetActive(false);



        var panel = GameObject.Find("Canvas").transform.GetChild(0);
        
        for (int j = 0; j < panel.transform.childCount; j++)
        {
            var child = panel.transform.GetChild(j);
            if (Char.IsDigit(child.name[0]))
            {
                int digit = int.Parse(child.name);
                if(digit != systemChoose)
                {
                    ChangeTransp(child.gameObject, 0.1f);
                    print("élément non sélect mis en transparence");
                }
                    
                else
                {
                    ChangeTransp(child.gameObject, 0.7f);
                    print("élément sélect mis en avant");
                    oldGo = child.gameObject;
                }
            }
        }



    }

    public async void SetPosition()
    {
        var position = await GetGpsPosition();
        _positions.Add(position);
        print("position gps ajoutée");
        _panelTropCourt.SetActive(false);
        _panelNewpointGPS.SetActive(true);
        _panelArbresSpawn.SetActive(false);
        _panelDistancentreLesDeuxPointsGPS.SetActive(false);
    }

    private void Interpol()
    {
        
        _panelEspacementValid.SetActive(false);
        _panelEspacementNonValid.SetActive(false);

        // Options pour le placement des objets
        var opts = new PlaceAtLocation.PlaceAtOptions()
        {
            HideObjectUntilItIsPlaced = true,
            MaxNumberOfLocationUpdates = 2,
            MovementSmoothing = 0.1f,
            UseMovingAverage = false,
        };
        
        
        //print("Interpolation");

        if (_positions.Count < 2)
        {
            print("At least two positions are required for interpolation.");
            return;
        }

        // Récupère les coordonnées lat-long des deux premières positions
        float lat1 = _positions[0]["lat"];
        float lon1 = _positions[0]["long"];
        float lat2 = _positions[1]["lat"];
        float lon2 = _positions[1]["long"];

        // Calcul de la distance entre les deux premières positions en mètres
        float distanceMeters = CalculateDistance(lat1, lon1, lat2, lon2);

        // Calcul du nombre d'arbres à placer entre les deux positions
        int numTrees = Mathf.Max(Mathf.RoundToInt(distanceMeters / espacement), 1); 

        print("Distance entre les deux points en m :");
        print(distanceMeters);
        print("Nombre d'arbres à placer :");
        print(numTrees);

        textMeshDistanceMetres.text = "Distance entre les deux points :  " + distanceMeters + "m";
        _panelDistancentreLesDeuxPointsGPS.SetActive(true);
        
        // Seuil de distance en dessous duquel on n'effectue pas d'interpolation
        ///float distanceThreshold = 3f; 


        float p = 0 / (float)(numTrees - 1);
        float interLat = Mathf.Lerp(lat1, lat2, p);
        float interLon = Mathf.Lerp(lon1, lon2, p);



        if (float.IsNaN(interLat) || float.IsNaN(interLon))
        {
            print("Distance entre les deux points trop petite pour l'interpolation.");
            


            if(ArbreOuHaie == "arbre")
            {
                GameObject treeClone = Instantiate(treePrefab);
                treeClone.tag = "treeClone";
                
                // Placer un arbre à cette position
                Location location = new Location(lat1, lon1, -12); 
                PlaceAtLocation.AddPlaceAtComponent(treeClone, location, opts);
                print("Arbre créé à la position actuelle (sans interpolation) : " + location);
                textMeshUnSeulCree.text = "Distance entre les deux points GPS trop courte : un seul arbre créé";
                _panelTropCourt.SetActive(true);     
            }

            if(ArbreOuHaie == "haie")
            {
                GameObject haieClone = Instantiate(haiePrefab);
                haieClone.tag = "treeClone";
                
                // Placer un arbre à cette position
                Location location = new Location(lat1, lon1, -12); 
                PlaceAtLocation.AddPlaceAtComponent(haieClone, location, opts);
                print("Haie créé à la position actuelle (sans interpolation) : " + location);
                textMeshUnSeulCree.text = "Distance entre les deux points GPS trop courte : une seule haie créée";
                _panelTropCourt.SetActive(true);     
            }
            

        }
        
        else 
        {
            if(ArbreOuHaie == "arbre")
            {
                // Interpolation entre les deux positions GPS
                for (int i = 0; i < numTrees; i++)
                {
                    float t = i / (float)(numTrees - 1);
                    float interpolatedLat = Mathf.Lerp(lat1, lat2, t);
                    float interpolatedLon = Mathf.Lerp(lon1, lon2, t);

                    double interpolatedLatDOUBLE = (double)interpolatedLat;
                    double interpolatedLonDOUBLE = (double)interpolatedLon;  


                    GameObject treeClone = Instantiate(treePrefab);
                    treeClone.tag = "treeClone";


                    // Placer un arbre à cette position
                    Location location = new Location(interpolatedLatDOUBLE, interpolatedLonDOUBLE, -12); 

                    PlaceAtLocation.AddPlaceAtComponent(treeClone, location, opts);

                    print("Arbre créé à la position interpolée : " + location);
                    textMeshProArbresSpawn.text = "Rangée de " + numTrees + " arbres créé, chacun espacé de " + espacement + "m";
                    _panelArbresSpawn.SetActive(true);
                }
            }


            if(ArbreOuHaie == "haie")
            {
                // Interpolation entre les deux positions GPS
                for (int i = 0; i < numTrees; i++)
                {
                    float t = i / (float)(numTrees - 1);
                    float interpolatedLat = Mathf.Lerp(lat1, lat2, t);
                    float interpolatedLon = Mathf.Lerp(lon1, lon2, t);

                    double interpolatedLatDOUBLE = (double)interpolatedLat;
                    double interpolatedLonDOUBLE = (double)interpolatedLon;  


                    GameObject haieClone = Instantiate(haiePrefab);
                    haieClone.tag = "haieClone";


                    // Placer un arbre à cette position
                    Location location = new Location(interpolatedLatDOUBLE, interpolatedLonDOUBLE, -12); 

                    PlaceAtLocation.AddPlaceAtComponent(haieClone, location, opts);

                    print("Haie créé à la position interpolée : " + location);
                    textMeshProArbresSpawn.text = "Rangée de " + numTrees + " haies créé, chacune espacée de " + espacement + "m";
                    _panelArbresSpawn.SetActive(true);
                }
            }

        }
        
    }
    



    void Update()
    {
        
        if (_positions.Count >= 2)
        {
            print("c'est bon j'ai une deuxième position");
            _panelNewpointGPS.SetActive(false);
            Interpol();
            _positions.Clear();
        }
    }
    
    private async Task<Dictionary<string, float>> GetGpsPosition()
    {
        var dict = new Dictionary<string, float>()
        {
            {
                "lat", 0
            },
            {
                "long", 0
            },
            {
                "alt", 0
            }
        };
        
        if(!Input.location.isEnabledByUser)
        {
            print("location not enabled => permission request");
            Permission.RequestUserPermission(Permission.FineLocation);
        }
        
        if(!Input.location.isEnabledByUser)
            return dict;
        
        
        if(Input.location.status != LocationServiceStatus.Running)
            Input.location.Start(); 

        // Waits until the location service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            await Task.Delay(1000);
            maxWait--;
        }

        // If the service didn't initialize in 20 seconds this cancels location service use.
        if (maxWait < 1)
        {
            print("Timed out");
            return dict;
        }

        // If the connection failed this cancels location service use.
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            print("Unable to determine device location");
            return dict;
        }
        
        dict["lat"] = Input.location.lastData.latitude;
        dict["long"] = Input.location.lastData.longitude;
        dict["alt"] = Input.location.lastData.altitude;
        
        print($"{dict["lat"]}, {dict["long"]}, {dict["alt"]}");


        return dict;
    }



    // Fonction pour calculer la distance en mètres entre deux points donnés en latitude et longitude
    private float CalculateDistance(float lat1, float lon1, float lat2, float lon2)
    {
        const float EarthRadius = 6371000; // Rayon de la Terre en mètres

        // Conversion des coordonnées de degrés à radians
        lat1 *= Mathf.Deg2Rad;
        lon1 *= Mathf.Deg2Rad;
        lat2 *= Mathf.Deg2Rad;
        lon2 *= Mathf.Deg2Rad;

        // Formule de Haversine pour calculer la distance
        float dlat = lat2 - lat1;
        float dlon = lon2 - lon1;

        float a = Mathf.Sin(dlat / 2) * Mathf.Sin(dlat / 2) +
                Mathf.Cos(lat1) * Mathf.Cos(lat2) *
                Mathf.Sin(dlon / 2) * Mathf.Sin(dlon / 2);

        float c = 2 * Mathf.Atan2(Mathf.Sqrt(a), Mathf.Sqrt(1 - a));

        float distance = EarthRadius * c; // Distance en mètres
        return distance;
    }





    /////////////////////////////////
    
    public void ValiderEspacement()
    {
        // Lire la valeur de l'InputField et essayer de la convertir en float
        if (float.TryParse(inputEspacement.text, out float nouvelleValeurEspacement))
        {
            // Mettre à jour la variable espacement avec la nouvelle valeur
            espacement = nouvelleValeurEspacement;
            Debug.Log("Nouvelle valeur d'espacement : " + espacement);
            _panelEspacementValid.SetActive(true);
            _panelEspacementNonValid.SetActive(false);
            //yield return new WaitForSeconds(3.0f);
            //_panelEspacementValid.SetActive(false);

        }
        else
        {
            Debug.LogWarning("La valeur d'espacement n'est pas valide.");
            _panelEspacementNonValid.SetActive(true);
            _panelEspacementValid.SetActive(false);
            //yield return new WaitForSeconds(3.0f);
            //_panelEspacementNonValid.SetActive(false);
        }
    }

    /////////////////////////////////





    public void BtnSystem()
    {
        var go = EventSystem.current.currentSelectedGameObject;
        
        if(go == oldGo)
        
            return;
        
        systemChoose = int.Parse(go.name);
        ChangeTransp(go, 0.7f);
        print(ArbreOuHaie);
        
            
        
        if(oldGo != null)
        {
            ChangeTransp(oldGo, 0.1f);
            if (ArbreOuHaie == "arbre")
            {
                ArbreOuHaie = "haie";  
            }
            else if (ArbreOuHaie == "haie")
            {
                ArbreOuHaie = "arbre"; 
            }

            oldGo = go;
            print(ArbreOuHaie);
        }
            
    }



    void ChangeTransp(GameObject go, float transparency)
    {
        var rawImage = go.transform.GetComponentInChildren<RawImage>();
        var image = go.GetComponent<Image>();

        rawImage.color = new Color(rawImage.color.r, rawImage.color.g, rawImage.color.b, transparency); 
    
        image.color = new Color(image.color.r, image.color.g, image.color.b, transparency);
            
    }







}
