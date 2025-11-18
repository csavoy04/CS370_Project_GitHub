using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class Main_Menu_Controller : MonoBehaviour
{

    [SerializeField] UIDocument uiDocument;
    public VisualTreeAsset listItemTemplate;
    private VisualElement root;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        root = uiDocument.rootVisualElement;


        //Closes Party Menu and Bag Menu at start
        var menuParty = root.Q<VisualElement>("Party-Menu");
        menuParty.style.display = DisplayStyle.None;
        var menuBag = root.Q<VisualElement>("Bag-Menu");
        menuBag.style.display = DisplayStyle.None;

        //Closes Menu at start and when opened will only show main menu
        root.style.display = DisplayStyle.None;

        //Main Menu Buttons
        var playBtn = root.Q<VisualElement>("PlayBtn");
        playBtn.RegisterCallback<ClickEvent>(PlayEvent);

        var PartyBtn = root.Q<VisualElement>("PartyBtn");
        PartyBtn.RegisterCallback<ClickEvent>(PartyEvent);

        var BagBtn = root.Q<VisualElement>("BagBtn");
        BagBtn.RegisterCallback<ClickEvent>(BagEvent);

        var quitBtn = root.Q<VisualElement>("QuitBtn");
        quitBtn.RegisterCallback<ClickEvent>(QuitEvent);

        //Party Buttons
        var partyBack = root.Q<VisualElement>("PartyBackBtn");
        partyBack.RegisterCallback<ClickEvent>(PartyBackEvent);

        //Bag Buttons
        var bagBack = root.Q<VisualElement>("BagBackBtn");
        bagBack.RegisterCallback<ClickEvent>(PartyBackEvent);
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
        //Unit 1
        var unit1Name = root.Q<Label>("Unit1-Name");
        unit1Name.text = PartySystem.Instance.PlayerParty[0].Name;

        var unit1HealthBar = root.Q<ProgressBar>("Unit1-Health-Bar");
        unit1HealthBar.value = PartySystem.Instance.PlayerParty[0].CurrentHealth;
        unit1HealthBar.highValue = PartySystem.Instance.PlayerParty[0].MaxHealth;

        var unit1HpNum = root.Q<Label>("Unit1-Health-Num");
        unit1HpNum.text = "HP:" + PartySystem.Instance.PlayerParty[0].CurrentHealth + "/" + PartySystem.Instance.PlayerParty[0].MaxHealth;

        var unit1StatsNum = root.Q<Label>("Unit1-Stats-Num");
        unit1StatsNum.text = "ATK:" + PartySystem.Instance.PlayerParty[0].CurrentAttack + "  DEF:" + PartySystem.Instance.PlayerParty[0].CurrentDefense + "  SPD:" + PartySystem.Instance.PlayerParty[0].CurrentSpeed;

        //Unit 2
        var unit2Info = root.Q<VisualElement>("Unit2-Info");

        if (PartySystem.Instance.PlayerParty[1] != null || PartySystem.Instance.PlayerParty[1].ClassType != Unit.UnitClass.Empty)
        {
            unit2Info.style.display = DisplayStyle.Flex;
            var unit2Name = root.Q<Label>("Unit2-Name");
            unit2Name.text = PartySystem.Instance.PlayerParty[1].Name;

            var unit2HealthBar = root.Q<ProgressBar>("Unit2-Health-Bar");
            unit2HealthBar.value = PartySystem.Instance.PlayerParty[1].CurrentHealth;
            unit2HealthBar.highValue = PartySystem.Instance.PlayerParty[1].MaxHealth;

            var unit2HpNum = root.Q<Label>("Unit2-Health-Num");
            unit2HpNum.text = "HP:" + PartySystem.Instance.PlayerParty[1].CurrentHealth + "/" + PartySystem.Instance.PlayerParty[1].MaxHealth;

            var unit2StatsNum = root.Q<Label>("Unit2-Stats-Num");
            unit2StatsNum.text = "ATK:" + PartySystem.Instance.PlayerParty[1].CurrentAttack + "  DEF:" + PartySystem.Instance.PlayerParty[1].CurrentDefense + "  SPD:" + PartySystem.Instance.PlayerParty[1].CurrentSpeed;
        }
        else
        {
            unit2Info.style.display = DisplayStyle.None;
        }

        //Unit 3
        var unit3Info = root.Q<VisualElement>("Unit3-Info");

        if (PartySystem.Instance.PlayerParty[2] != null || PartySystem.Instance.PlayerParty[2].ClassType != Unit.UnitClass.Empty)
        {
            unit3Info.style.display = DisplayStyle.Flex;
            var unit3Name = root.Q<Label>("Unit3-Name");
            unit3Name.text = PartySystem.Instance.PlayerParty[2].Name;

            var unit3HealthBar = root.Q<ProgressBar>("Unit3-Health-Bar");
            unit3HealthBar.value = PartySystem.Instance.PlayerParty[2].CurrentHealth;
            unit3HealthBar.highValue = PartySystem.Instance.PlayerParty[2].MaxHealth;

            var unit3HpNum = root.Q<Label>("Unit3-Health-Num");
            unit3HpNum.text = "HP:" + PartySystem.Instance.PlayerParty[2].CurrentHealth + "/" + PartySystem.Instance.PlayerParty[2].MaxHealth;

            var unit3StatsNum = root.Q<Label>("Unit3-Stats-Num");
            unit3StatsNum.text = "ATK:" + PartySystem.Instance.PlayerParty[2].CurrentAttack + "  DEF:" + PartySystem.Instance.PlayerParty[2].CurrentDefense + "  SPD:" + PartySystem.Instance.PlayerParty[2].CurrentSpeed;
        }
        else
        {
            unit3Info.style.display = DisplayStyle.None;
        }
    }

    private void BagEvent(ClickEvent evt)
    {
        var menuMain = root.Q<VisualElement>("Player-Menu");
        menuMain.style.display = DisplayStyle.None;

        var menuBag = root.Q<VisualElement>("Bag-Menu");
        menuBag.style.display = DisplayStyle.Flex;

        ListView itemList = root.Q<ListView>("Item-List");
        var bagItems = new List<string>()
        {
            "One", "Two"
        };
        Debug.Log("Make item called");
        //How many in list
        itemList.itemsSource = bagItems;

        //Creates Item
        itemList.makeItem = () =>
        {
            return listItemTemplate.CloneTree();
        };

        //Binds to label
        itemList.bindItem = (element, index) =>
        {
            Label label = element.Q<Label>("Name");
            label.text = bagItems[index];
        };

        itemList.selectionType = SelectionType.Single;
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

        var menuBag = root.Q<VisualElement>("Bag-Menu");
        menuBag.style.display = DisplayStyle.None;

        var menuMain = root.Q<VisualElement>("Player-Menu");
        menuMain.style.display = DisplayStyle.Flex;

    }
}
