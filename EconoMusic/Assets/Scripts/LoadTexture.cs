﻿//Alex Jungroth
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadTexture : MonoBehaviour{

	public FileBrowser browser;
	public FileSelectMode mode;
	public string[] searchPatterns;
	FileInfo sel;
	string path = "";

	//Holds the file path so textures can be loaded in (AAJ)
	string filePath;

	//Holds the name of the file so it can be used to display on an instrument (AAJ)
	public string fileName;

	//Holds the default image for the textures (AAJ)
	public Sprite defaultPlusImage;

	//Holds the sprite that will be loaded into this object (AAJ)
	public Sprite loadedSprite;

	//Prevents more than one texture from being loaded on the same object (AAJ)
	public bool loaded = false;

	//Holds the prefab for a new slot (AAJ)
	public GameObject slot;

	//Holds a the panel where the slots are stored (AAJ)
	public GameObject texturePanel;

	//Holds a button that will delete a loaded texture (AAJ)
	public GameObject deleteButton;

	//Holds a color mask so it can be enabled and disabled (AAJ)
	public GameObject colorMask;

	//Holds an import image so it can be enabled and disabled (AAJ)
	public GameObject importImage;

	//Holds the text box for displaying the images file name (AAJ)
	public Text fileNameText;

	//Holds the master insturment so it can be enabled (AAJ)
	private GameObject[] instruments;

	// Use this for initialization
	void Start ()
	{
		//Resets the loaded variable for new instantiations of slot (AAJ)
		loaded = false;

		//Sets the texture to the default image when it is generated (AAJ)
		GetComponent<Image>().sprite = defaultPlusImage;

		//Clears the textures text box (AAJ)
		fileNameText.text = "";

	}//Start

	// Update is called once per frame
	void Update(){

		//Updates the images so that they have the proper attributes (AAJ)
		if(loaded == true){
		
			deleteButton.SetActive(true);
			colorMask.SetActive(true);
			importImage.SetActive(true);
		}//if 
		else{

			deleteButton.SetActive(false);
			colorMask.SetActive(false);
			importImage.SetActive(false);
		}//else
	}//update

	public void OnMouseDown(){

		if(loaded == false){

			if(!browser.isShowing){

				browser.Show(path, searchPatterns, this, mode);
			}
		
			//Finds the instruments (AAJ)
			instruments = GameObject.FindGameObjectsWithTag("Instrument");
			
			//Moves any instruments that were moved up back down (AAJ)
			for(int i = 0; i < instruments.Length; i++){
				
				instruments[i].GetComponent<InstrumentScript>().MoveInsturmentUp();
			}//for
		}//if
	}//OnMouseDown
	
	// The FileBrowser will send a message to this MonoBehaviour when the user selects a file
	// Set the 'SelectEventName' in the inspector to the name of the function you want to receive the message
	public void OnFileSelected(FileInfo info){
			
		sel = info;
			
		//Loads in the texture (AAJ)
		load(sel.path);
			
		if (instruments != null) {
			//Moves any instruments that were moved up back down (AAJ)
			for (int i = 0; i < instruments.Length; i++) {
			
				instruments [i].GetComponent<InstrumentScript> ().MoveInsturmentDown ();
			}//for
		}
	}//OnFileSelected
	
	public void OnFileChange(FileInfo file){

		//Gets the name of the file so it can be displayed on the instrument (AAJ)
		fileName = file.name;

		//Debug.Log("File section changed to: " + file.name);
	}//OnFileChange
	
	void OnBrowseCancel(){

		//Moves any instruments that were moved up back down (AAJ)
		for(int i = 0; i < instruments.Length; i++){
			
			instruments[i].GetComponent<InstrumentScript>().MoveInsturmentDown();
		}//for

		//Debug.Log("You have cancelled");
	}//OnBrowseCancel

	/// <summary>
	/// Load the specified texture. (AAJ)
	/// </summary>
	public void load(string filePath){

		//Prevents multiple textures being loaded onto the same object
		//but only once something has been loaded (AAJ)
		loaded = true;

		//Puts the file name on the images' text component (AAJ)
		fileNameText.text = fileName;

		//Gets the sprite from the file path (AAJ)
		byte[] bytes = System.IO.File.ReadAllBytes(filePath);
		Texture2D tex = new Texture2D(1, 1);
		tex.LoadImage(bytes);

		//Generates a sprite dynamically (AAJ)
		Rect rect = new Rect(0, 0, tex.width, tex.height);
		Vector2 pivot = new Vector2(0.5f, 0.5f);
		loadedSprite = Sprite.Create(tex, rect, pivot);

		//Loads the new sprite into the object's sprite component (AAJ)
		GetComponent<Image>().sprite = loadedSprite;
		
		//instantiates a new slot and texture that can load another image (AAJ)
		GameObject newSlot = Instantiate(slot, new Vector3(0,0,0), Quaternion.identity) as GameObject;

		//sets the new slots parent to the slot panel (AAJ)
		newSlot.transform.SetParent(texturePanel.transform, false);
	}//Load

	/// <summary>
	/// Deletes the texture. (AAJ)
	/// </summary>
	public void DeleteTexture() {

		//the texture destroys its parent, its children and itself in the process (AAJ)
		Destroy(transform.parent.gameObject);

	}//DeleteTexture
}//LoadTexture