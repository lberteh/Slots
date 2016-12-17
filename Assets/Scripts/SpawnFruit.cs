using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SpawnFruit : MonoBehaviour {

    public GameObject reel;
    public GameObject[] reels;
    public Sprite[] fruitSprites;
    public SlotMachine sm;
    public GameObject levelManager;
    private double playerMoney = 500;
    private double winnings = 0;
    private double jackpot = 5000;
    private float turn = 0.0f;
    private double playerBet = 10;
    private float winNumber = 0.0f;
    private float lossNumber = 0.0f;
    private string[] spinResult;
    private string fruits = "";
    private float winRatio = 0.0f;
    private float lossRatio = 0.0f;
    private int grapes = 0;
    private int bananas = 0;
    private int oranges = 0;
    private int cherries = 0;
    private int bars = 0;
    private int bells = 0;
    private int sevens = 0;
    private int blanks = 0;

    public Button spinButton;
    public Text totalCreditsText, betText, winnerPaidText, generalText;

    void Start()
    {
        spinButton = GameObject.Find("spinButton").GetComponent<Button>();
        totalCreditsText = GameObject.Find("totalCreditsText").GetComponent<Text>();      
        betText = GameObject.Find("betText").GetComponent<Text>();
        winnerPaidText = GameObject.Find("winnerPaidText").GetComponent<Text>();
        generalText = GameObject.Find("generalText").GetComponent<Text>();
        
        StartCoroutine(initialUIAnimation());
       
    }

    void Update()
    {
        if (playerMoney == 0)
        {
            spinButton.interactable = false;
        }
    }

    public void reset()
    {
        spinButton.interactable = false;
        Button[] betButtons = GameObject.Find("Bet Buttons").GetComponentsInChildren<Button>();
        resetAll();

        foreach (Button btn in betButtons)
        {
            btn.interactable = false;
        }

        StartCoroutine(initialUIAnimation());
    }

    private IEnumerator initialUIAnimation()
    {
        var timer = new[] { 0.5f, 0.5f, 0.5f, 0.5f, 0.5f,
            0.5f, 0.5f, 0.5f, 0.5f};

        totalCreditsText.text = "-- -- --";
        betText.text = "-- --";
        winnerPaidText.text = "-- -- --";
        generalText.text = "-- -- --";

        for (int i = 0; i < timer.Length; i++)
        {
            if (i == timer.Length - 1)
            {
                updateUIText();
                spinButton.interactable = true;
                Button[] betButtons = GameObject.Find("Bet Buttons").GetComponentsInChildren<Button>();

                foreach (Button btn in betButtons)
                {
                    btn.interactable = true;
                }
            }
                

            if (i%2 != 0 )
            {
                totalCreditsText.enabled = false;
                betText.enabled = false;
                winnerPaidText.enabled = false;
                generalText.enabled = false;
            }
            else
            {
                totalCreditsText.enabled = true;
                betText.enabled = true;
                winnerPaidText.enabled = true;
                generalText.enabled = true;
            }                
            
            yield return new WaitForSeconds(timer[i]);
        }
    }

    private void updateUIText()
    {
        totalCreditsText.text = playerMoney.ToString("C2");
        betText.text = playerBet.ToString("C2");
        winnerPaidText.text = 0.ToString("C2");
        generalText.text = "Good Luck!";
    }

    public void updateBetAmount()
    {
        Button clicked = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        playerBet = System.Convert.ToInt32(clicked.tag);
        betText.text = playerBet.ToString("C2");
    }

    /* Utility function to show Player Stats */
    private void showPlayerStats()
    {
        winRatio = winNumber / turn;
        lossRatio = lossNumber / turn;
        string stats = "";
        stats += ("Jackpot: " + jackpot + "\n");
        stats += ("Player Money: " + playerMoney + "\n");
        stats += ("Turn: " + turn + "\n");
        stats += ("Wins: " + winNumber + "\n");
        stats += ("Losses: " + lossNumber + "\n");
        stats += ("Win Ratio: " + (winRatio * 100) + "%\n");
        stats += ("Loss Ratio: " + (lossRatio * 100) + "%\n");
        Debug.Log(stats);
    }

    /* Utility function to reset all fruit tallies*/
    private void resetFruitTally()
    {
        grapes = 0;
        bananas = 0;
        oranges = 0;
        cherries = 0;
        bars = 0;
        bells = 0;
        sevens = 0;
        blanks = 0;
    }

    /* Utility function to reset the player stats */
    private void resetAll()
    {
        playerMoney = 1000;
        winnings = 0;
        jackpot = 5000;
        turn = 0;
        playerBet = 10;
        winNumber = 0;
        lossNumber = 0;
        winRatio = 0.0f;


    }

    /* Check to see if the player won the jackpot */
    private void checkJackPot()
    {
        /* compare two random values */
        var jackPotTry = Random.Range(1, 10);
        var jackPotWin = Random.Range(1, 10);
        if (jackPotTry == jackPotWin)
        {
            Debug.Log("You Won the $" + jackpot + " Jackpot!!");
            playerMoney += jackpot;
            totalCreditsText.text = playerMoney.ToString("C2");
            StartCoroutine(displayJackpotMessage());
            jackpot = 1000;
        }
    }

    private IEnumerator displayJackpotMessage()
    {
        var timer = new[] { 0.5f, 0.5f, 0.5f, 0.5f, 0.5f,
            0.5f, 0.5f, 0.5f, 0.5f};

        generalText.text = "*_JACKPOT_*";

        for (int i = 0; i < timer.Length; i++)
        {
            if (i == timer.Length - 1)
            {
                generalText.text = "Good Luck!";
            }


            if (i % 2 != 0)
            {                
                generalText.enabled = false;
            }
            else
            {                
                generalText.enabled = true;
            }

            yield return new WaitForSeconds(timer[i]);
        }
    }

    /* Utility function to show a win message and increase player money */
    private void showWinMessage()
    {
        playerMoney += winnings;
        totalCreditsText.text = playerMoney.ToString("C2");
        winnerPaidText.text = winnings.ToString("C2");
        Debug.Log("You Won: $" + winnings);
        resetFruitTally();
        checkJackPot();
    }

    /* Utility function to show a loss message and reduce player money */
    private void showLossMessage()
    {
        playerMoney -= playerBet;
        jackpot = jackpot + (playerBet*0.1);
        totalCreditsText.text = playerMoney.ToString("C2");
        winnerPaidText.text = 0.ToString("C2");
        Debug.Log("You Lost!");
        resetFruitTally();
        if (playerMoney == 0)
        {
            generalText.text = "You Lost!";
        }
    }

    /* Utility function to check if a value falls within a range of bounds */
    private bool checkRange(int value, int lowerBounds, int upperBounds)
    {
        return (value >= lowerBounds && value <= upperBounds) ? true : false;

    }

    /* When this function is called it determines the betLine results.
    e.g. Bar - Orange - Banana */
    private string[] Reels()
    {
        string[] betLine = { " ", " ", " " };
        int[] outCome = { 0, 0, 0 };

        for (var spin = 0; spin < 3; spin++)
        {
            outCome[spin] = Random.Range(1, 65);

            if (checkRange(outCome[spin], 1, 27))
            {  // 41.5% probability
                betLine[spin] = "blank";
                blanks++;
            }
            else if (checkRange(outCome[spin], 28, 37))
            { // 15.4% probability
                betLine[spin] = "Grapes";
                grapes++;
            }
            else if (checkRange(outCome[spin], 38, 46))
            { // 13.8% probability
                betLine[spin] = "Banana";
                bananas++;
            }
            else if (checkRange(outCome[spin], 47, 54))
            { // 12.3% probability
                betLine[spin] = "Orange";
                oranges++;
            }
            else if (checkRange(outCome[spin], 55, 59))
            { //  7.7% probability
                betLine[spin] = "Cherry";
                cherries++;
            }
            else if (checkRange(outCome[spin], 60, 62))
            { //  4.6% probability
                betLine[spin] = "Bar";
                bars++;
            }
            else if (checkRange(outCome[spin], 63, 64))
            { //  3.1% probability
                betLine[spin] = "Bell";
                bells++;
            }
            else if (checkRange(outCome[spin], 65, 65))
            { //  1.5% probability
                betLine[spin] = "Seven";
                sevens++;
            }

        }
        return betLine;
    }

    /* This function calculates the player's winnings, if any */
    private void determineWinnings()
    {
        if (blanks == 0)
        {
            if (grapes == 3)
            {
                winnings = playerBet * 10;
            }
            else if (bananas == 3)
            {
                winnings = playerBet * 20;
            }
            else if (oranges == 3)
            {
                winnings = playerBet * 30;
            }
            else if (cherries == 3)
            {
                winnings = playerBet * 40;
            }
            else if (bars == 3)
            {
                winnings = playerBet * 50;
            }
            else if (bells == 3)
            {
                winnings = playerBet * 75;
            }
            else if (sevens == 3)
            {
                winnings = playerBet * 100;
            }
            else if (grapes == 2)
            {
                winnings = playerBet * 2;
            }
            else if (bananas == 2)
            {
                winnings = playerBet * 2;
            }
            else if (oranges == 2)
            {
                winnings = playerBet * 3;
            }
            else if (cherries == 2)
            {
                winnings = playerBet * 4;
            }
            else if (bars == 2)
            {
                winnings = playerBet * 5;
            }
            else if (bells == 2)
            {
                winnings = playerBet * 10;
            }
            else if (sevens == 2)
            {
                winnings = playerBet * 20;
            }
            else if (sevens == 1)
            {
                winnings = playerBet * 5;
            }
            else
            {
                winnings = playerBet * 1;
            }
            winNumber++;
            showWinMessage();
        }
        else
        {
            lossNumber++;
            showLossMessage();
        }

    }

    public void OnSpinButtonClick()
    {
        playerBet = 10; // default bet amount

        if (playerMoney == 0)
        {
            /*
			if (Debug.Log("You ran out of Money! \nDo you want to play again?","Out of Money!",MessageBoxButtons.YesNo) == DialogResult.Yes)
			{
				resetAll();
				showPlayerStats();
			}
			*/
        }
        else if (playerBet > playerMoney)
        {
            Debug.Log("You don't have enough Money to place that bet.");
        }
        else if (playerBet < 0)
        {
            Debug.Log("All bets must be a positive $ amount.");
        }
        else if (playerBet <= playerMoney)
        {
            spinResult = Reels();
            fruits = spinResult[0] + " - " + spinResult[1] + " - " + spinResult[2];
            Debug.Log(fruits);
            determineWinnings();
            turn++;
            showPlayerStats();
        }
        else
        {
            Debug.Log("Please enter a valid bet amount");
        }
    }

    private void checkIfPlayerCanBet()
    {
        Button[] betButtons = GameObject.Find("Bet Buttons").GetComponentsInChildren<Button>();

        foreach (Button btn in betButtons)
        {
            if (System.Convert.ToInt32(btn.tag) > playerMoney)
            {            
                btn.interactable = false;
                
                if (playerBet == System.Convert.ToInt32(btn.tag))
                {
                    foreach (Button button in betButtons)
                    {
                        if (button.interactable == true)
                        {
                            playerBet = System.Convert.ToInt32(button.tag);
                            betText.text = playerBet.ToString("C2");
                        }
                    }
                }
            }
            else
            {            
                btn.interactable = true;
            }

             

        }
    }


    public void DisplayRandomFruit()
    {
             
        reels = GameObject.FindGameObjectsWithTag("Reel");
        
        foreach (GameObject reel in reels)
        {
            //int randomIndex = Random.Range(0, fruitSprites.Length);
            //Sprite fruitSprite = fruitSprites[randomIndex];
            int spriteIndex = 0;
            int fruitRange = Random.Range(1, 65);

            if (checkRange(fruitRange, 1, 27))
            {  // 41.5% probability
                spriteIndex = 0; // blank
                blanks++;
            }
            else if (checkRange(fruitRange, 28, 37))
            { // 15.4% probability
                spriteIndex = 1; // grape
                grapes++;
            }
            else if (checkRange(fruitRange, 38, 46))
            { // 13.8% probability
                spriteIndex = 2; // banana
                bananas++;
            }
            else if (checkRange(fruitRange, 47, 54))
            { // 12.3% probability
                spriteIndex = 3; // orange
                oranges++;
            }
            else if (checkRange(fruitRange, 55, 59))
            { //  7.7% probability
                spriteIndex = 4; // cherry
                cherries++;
            }
            else if (checkRange(fruitRange, 60, 62))
            { //  4.6% probability
                spriteIndex = 5; // bar
                bars++;
            }
            else if (checkRange(fruitRange, 63, 64))
            { //  3.1% probability
                spriteIndex = 6; // bell
                bells++;
            }
            else if (checkRange(fruitRange, 65, 65))
            { //  1.5% probability
                spriteIndex = 7; // seven
                sevens++;
            }

            Sprite fruitSprite = fruitSprites[spriteIndex];
            reel.GetComponent<SpriteRenderer>().sprite = fruitSprite;

            // Debug.Log(fruitSprite.name);
        }

        
        
    }

    // when spin button is clicked
    public void CallRepeatTimer()
    {        
        StartCoroutine(RepeatTimer());            
        spinButton.interactable = false;                 
    }

    public IEnumerator RepeatTimer()
    {
        var timer = new[] { 0.1f, 0.1f, 0.1f, 0.1f, 0.1f,
            0.2f, 0.2f, 0.2f, 0.2f}; // ,  0.3f, 0.3f, 0.3f, 0.4f, 0.4f, 0.5f, 0.5f,  0.6f, 0.8f, 1.0f, 1.5f 

        // spin reels (ui)
        for (int i = 0; i < timer.Length; i++)
        {
            if (i == timer.Length - 1)
                resetFruitTally();
            DisplayRandomFruit();
            // Debug.Log("I'm in!!! " + i); //<-- add a x to test this
            yield return new WaitForSeconds(timer[i]);
        }

        // check result 
        determineWinnings();
        checkIfPlayerCanBet();
        Debug.Log(playerMoney);
        spinButton.interactable = true;

    }



}
