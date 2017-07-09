using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStateManager : MonoBehaviour {

	GameState state;

	public PlayerHealth playerHealth;
	public Text gameoverText;
	public InventorySystem inventorySystem;
	public SpatialUI alma;
	public Text almaText;
	public GameObject helper;
	public ProbeOrb orb;

	public Image grabHint;
	public Image pushHint;
	public Image bagHint;

	public int maskCount = 0;
	public bool waterRiddled,bridgeRiddled;
	public bool orbReceived,jadeCollected,realmOpened,deskaroExposed,deskaroOut,deskaroFaint,maskLost,maskOne,maskThree,masksPurified,patternMissing,patternComplete,doorOpen;

	public enum GameState {START,GAMEOVER,PAUSE,INTRO,ORB_RECEIVED,JADE_COLLECTED,
		REALM_OPEN,DESKARO_EXPOSED,DESKARO_OUT,DESKARO_FAINT,LOST_MASK,MASK_ONE,MASK_THREE,
		WATER_RIDDLE,PATTERN_MISSING,PATTERN_COMP,FINISH};

	string[] texts;
	int textIndex = 0;
	Image hint;

	// Use this for initialization
	void Start () {
		state = GameState.START;
	}

	void CheckPattern(){
		if (!masksPurified || !patternComplete) {
			GameObject[] masks = GameObject.FindGameObjectsWithTag ("Mask");
			int rawMaskCount = 0;
			int maskCount = 0;
			int smCount = 0,tsCount = 0,wCount = 0;
			if (masks.Length > 0) {
				for (int i = 0; i < masks.Length; i++) {
					Mask m = masks [i].GetComponent<Mask> ();
					if (m.detached) {
						maskCount++;
						if (m.purified) {
							if (m.maskType == Altoria.PatternType.SunMoon)
								smCount++;
							if (m.maskType == Altoria.PatternType.TimeSpace)
								tsCount++;
							if (m.maskType == Altoria.PatternType.War)
								wCount++;
						} else
							rawMaskCount++;
					}
				}
			}
			if (maskCount > 0 && rawMaskCount == 0) {
				masksPurified = true;
				if (smCount > 0 && tsCount > 0 && wCount > 0)
					patternComplete = true;
				else
					patternMissing = true;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		CheckPattern ();
		ContinueState ();
		if (playerHealth.HP <= 0) {
			state = GameState.GAMEOVER;
			gameoverText.enabled = true;
			StartCoroutine (Restart ());
		}
		else
			gameoverText.enabled = false;


		if (Input.GetButtonUp ("ButtonX")) {
			if (!alma.isOpen)
				inventorySystem.Toggle ();
		}
		if (Input.GetButtonUp ("ButtonY")) {
			if (alma.isOpen) {
				textIndex++;
				if (textIndex < texts.Length) {
					almaText.text = texts [textIndex];
				} else {
					alma.Close ();
					if (hint != null)
						StartCoroutine (ShowHint ());
				}
			} else {
				textIndex = 0;
				if (texts.Length > 0)
					almaText.text = texts [0];
				alma.Open ();
			}
			if (inventorySystem.isOpen)
				inventorySystem.Close ();
		}
		if (Input.GetButtonUp ("ButtonMenu")) {
			helper.SetActive (!helper.activeSelf);
		}
		if (Input.GetKeyDown (KeyCode.P)) {
			Time.timeScale = 1;
		}
	}

	IEnumerator Restart(){
		yield return new WaitForSeconds (5);
		SceneManager.LoadScene (1);
	}

	IEnumerator ShowHint(){
		float a = 0;
		float t = 0;
		Image curHint = hint;
		while (t < 4) {
			t += Time.deltaTime;
			if (t < 3.5f) {
				a += Time.deltaTime * 2; 
				if (a > 1)
					a = 1;
			} else {
				a -= Time.deltaTime * 2; 
				if (a < 0)
					a = 0;
			}
			curHint.color = new Color (curHint.color.r, curHint.color.g, curHint.color.b, a);
			yield return null;
		}
		curHint.color = new Color (curHint.color.r, curHint.color.g, curHint.color.b, 0);
	}

	void ContinueState(){
		switch (state) {
		case GameState.START:
			StartCoroutine (EnterStateAfter (GameState.INTRO, 2f));
			break;
		case GameState.INTRO:
			if (orbReceived)
				EnterState (GameState.ORB_RECEIVED);
			break;
		case GameState.ORB_RECEIVED:
			if (jadeCollected)
				EnterState (GameState.JADE_COLLECTED);
			break;
		case GameState.JADE_COLLECTED:
			if (realmOpened)
				EnterState (GameState.REALM_OPEN);
			break;
		case GameState.REALM_OPEN:
			if (deskaroExposed)
				EnterState (GameState.DESKARO_EXPOSED);
			break;
		case GameState.DESKARO_EXPOSED:
			if (deskaroOut)
				EnterState (GameState.DESKARO_OUT);
			break;
		case GameState.DESKARO_OUT:
			if (deskaroFaint)
				EnterState (GameState.DESKARO_FAINT);
			break;
		case GameState.DESKARO_FAINT:
			if (maskOne)
				EnterState (GameState.MASK_ONE);
			else if (maskLost)
				EnterState (GameState.LOST_MASK);
			break;
		case GameState.LOST_MASK:
			if (maskOne)
				EnterState (GameState.MASK_ONE);
			break;
		case GameState.MASK_ONE:
			if (maskThree)
				EnterState (GameState.MASK_THREE);
			if (patternMissing)
				EnterState (GameState.PATTERN_MISSING);
			break;
		case GameState.MASK_THREE:
			if (patternMissing)
				EnterState (GameState.PATTERN_MISSING);
			else if (patternComplete)
				EnterState (GameState.PATTERN_COMP);
			break;
		case GameState.PATTERN_MISSING:
			if (patternComplete)
				EnterState (GameState.PATTERN_COMP);
			break;
		case GameState.PATTERN_COMP:
			if (doorOpen)
				EnterState (GameState.FINISH);
			break;
		}
	}

	IEnumerator EnterStateAfter(GameState gameState, float delay){
		state = gameState;
		yield return new WaitForSeconds (delay);
		EnterState (gameState);
	}

	void EnterState(GameState gameState){
		state = gameState;
		hint = null;
		
		switch (gameState) {
		case GameState.INTRO:
			texts = new string[] {
				"This is it, we’re here; The Temple of Montora. The last remaining entrance to Altoria.",
				"If I recall we’ll need to appease the statue of the gods with a loyal Deskaro mask in order to unlock",
				"the doors. In order to find them you can use this."
			};
			StartCoroutine(orb.ShowUp ());
			hint = grabHint;
			break;
		case GameState.ORB_RECEIVED:
			texts = new string[]{
				"This area is rich in Jade, so we’ll need some to power the Realm Orb. Go now and try to find some.", 
				"Use your sword to chip jade deposits. Remember to strike harder!"};
			break;
		case GameState.JADE_COLLECTED:
			texts = new string[]{
				"You got some jade power! Altorians used the orb to see into other Realms. Just toss it anywhere to open a small", 
				"portal and see beyond our world."};
			break;
		case GameState.REALM_OPEN:
			texts = new string[]{
				"Great my boy! Just don’t go too crazy tossing that thing around as the jade’s power can dissipate after just 3 uses.",
				"A Deskaros mask is said to hold the mark of the Altorian god they serve, so we’ll need them to unlock the",
				"Temple gates. They say these spirits are attracted to ancient objects and ruins. See if you can expose some",
				"Deskaros using the orb."};
			break;
		case GameState.DESKARO_EXPOSED:
			texts = new string[]{
				"Use your power to push it out of that realm!"};
			hint = pushHint;
			break;
		case GameState.DESKARO_OUT:
			texts = new string[]{
				"Here it comes! Prepare to knock the Deskaro into a daze and take his mask while he’s incapacitated!"};
			break;
		case GameState.DESKARO_FAINT:
			texts = new string[]{
				"Take his mask now and defeat it."};
			break;
		case GameState.LOST_MASK:
			texts = new string[]{
				"You lost the mask! Remember to take his mask while he’s incapacitated!"};
			break;
		case GameState.MASK_ONE:
			texts = new string[] {
				"There are 3 Altorian gods guarding the gate, we should collect more masks to make sure we find the right ones."
			};
			hint = bagHint;
			break;
		case GameState.MASK_THREE:
			texts = new string[]{
				"Hmm we have plenty of masks but they’re all blank. There’s a ritual I don’t quite recall in order to see the God the", 
				"Deskaro serves. Look around - there might be something more the orb can show us."};
			break;
		case GameState.PATTERN_MISSING:
			texts = new string[]{
				"There are 3 Altorian gods guarding the gate, but it looks like we're missing some of the required masks.",
				"We should collect more mask to ensure we have a complete set."};
			break;
		case GameState.PATTERN_COMP:
			texts = new string[]{
				"Let’s get to the Gods and place the masks!"};
			break;
		case GameState.FINISH:
			texts = new string[]{
				"Finally here we are! Let's start our journey!"};
			break;
		}

		if (inventorySystem.isOpen)
			inventorySystem.Close ();

		textIndex = 0;
		if (texts.Length > 0)
			almaText.text = texts [0];
		
		if (!alma.isOpen) {
			alma.Open ();
		}
	}

	public void WaterRiddle(){

		if (!waterRiddled) {
			waterRiddled = true;

			texts = new string[] {
				"There's a riddle, it says \"Beneath the waves a face shows truth.\""
			};

			textIndex = 0;
			if (texts.Length > 0)
				almaText.text = texts [0];

			if (!alma.isOpen) {
				alma.Open ();
			}
		}
	}

	public void BridgeRiddle(){

		if (!bridgeRiddled) {
			bridgeRiddled = true;

			texts = new string[] {
				"The words above the gate means \"The Path you seek is worlds away.\" Where's the path?"
			};

			textIndex = 0;
			if (texts.Length > 0)
				almaText.text = texts [0];

			if (!alma.isOpen) {
				alma.Open ();
			}
		}
	}
}
