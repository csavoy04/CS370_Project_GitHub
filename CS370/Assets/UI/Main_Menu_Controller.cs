using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class Main_Menu_Controller : MonoBehaviour
{

    [SerializeField] UIDocument uiDocument;
    private VisualElement root;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        root = uiDocument.rootVisualElement;

        //Closes Party Menu at start
        var menuParty = root.Q<VisualElement>("Party-Menu");
        menuParty.style.display = DisplayStyle.None;

        //Closes Menu at start and when opened will only show main menu
        root.style.display = DisplayStyle.None;

        //Main Menu Buttons
        var playBtn = root.Q<VisualElement>("PlayBtn");
        playBtn.RegisterCallback<ClickEvent>(PlayEvent);

        var PartyBtn = root.Q<VisualElement>("PartyBtn");
        PartyBtn.RegisterCallback<ClickEvent>(PartyEvent);

        var quitBtn = root.Q<VisualElement>("QuitBtn");
        quitBtn.RegisterCallback<ClickEvent>(QuitEvent);

        //Party Buttons
        var partyBack = root.Q<VisualElement>("PartyBackBtn");
        partyBack.RegisterCallback<ClickEvent>(PartyBackEvent);
    }

    // Updates per frame
    private void Update()
    {
        //closes the main menu with the M button
        if (Input.GetKeyDown(KeyCode.M))
        {

            if (root.style.display == DisplayStyle.None)
            {
                // Show the menu
                root.style.display = DisplayStyle.Flex;
                Debug.Log("Menu opened");
            }
            else
            {
                // Hides and Updates the menu
                root.style.display = DisplayStyle.None;
                Debug.Log("Menu closed");

                Debug.Log(PartySystem.Instance.PlayerParty[0].CurrentHealth);
            }
        }
    }

    private void PlayEvent(ClickEvent evt)
    {
        root.style.display = DisplayStyle.None;
    }

    private void PartyEvent(ClickEvent evt)
    {
        var menuMain = root.Q<VisualElement>("Player-Menu");
        menuMain.style.display = DisplayStyle.None;

        var menuParty = root.Q<VisualElement>("Party-Menu");
        menuParty.style.display = DisplayStyle.Flex;

        //Updates the Party Menu Stats to the current instance
        var unit1HealthBar = root.Q<ProgressBar>("Unit1-Health-Bar");
        unit1HealthBar.value = PartySystem.Instance.PlayerParty[0].CurrentHealth;
        unit1HealthBar.highValue = PartySystem.Instance.PlayerParty[0].MaxHealth;

        var unit1Name = root.Q<Label>("Unit1-Name");
        unit1Name.text = PartySystem.Instance.PlayerParty[0].Name;

        var unit1HpNum = root.Q<Label>("Unit1-Health-Num");
        unit1HpNum.text = "HP:" + PartySystem.Instance.PlayerParty[0].CurrentHealth + "/" + PartySystem.Instance.PlayerParty[0].MaxHealth;
    }

    private void QuitEvent(ClickEvent evt)
    {
        Debug.Log("Quitting Game");
        Application.Quit();
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
    }

    private void PartyBackEvent(ClickEvent evt)
    {
        var menuParty = root.Q<VisualElement>("Party-Menu");
        menuParty.style.display = DisplayStyle.None;

        var menuMain = root.Q<VisualElement>("Player-Menu");
        menuMain.style.display = DisplayStyle.Flex;

    }
}
