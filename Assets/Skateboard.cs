using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class Skateboard : MonoBehaviour
{
    public float trickTimer;
    public string trick;
    public Rigidbody2D skateboardBody;
    public float jumpForce;
    public bool isAirborn;
    public float trickDuration;
    private SpriteRenderer spriteRenderer;
    public float skateboardVelocity;
    private float cameraSpeed;
    public GameObject WinCanvas;
    public GameObject LoseCanvas;
    public Camera cam;
    public AudioSource soundEffects;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        trickTimer = 0.0f;
        skateboardBody = GetComponent<Rigidbody2D>();
        skateboardBody.velocity = new Vector2(skateboardVelocity, 0);
        isAirborn = false;
        spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/Skater");
        soundEffects = GetComponent<AudioSource>(); // Assign AudioSource component

        cameraSpeed = skateboardVelocity;

        WinCanvas.SetActive(false);
        LoseCanvas.SetActive(false);

        cam = Camera.main;
        cam.backgroundColor = Color.black;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 viewportPosition = cam.WorldToViewportPoint(transform.position);
        if (viewportPosition.x < 0 || viewportPosition.x > 1)
        {
            ShowLoseScreen();
        }

        if (WinCanvas.activeSelf || LoseCanvas.activeSelf)
        {
            return;
        }

        if (trickTimer > 0)
        {
            trickTimer -= Time.deltaTime;
        }

        if (trickTimer <= 0)
        {
            ResetTrickState();
        }

        //Jump!
        if (Input.GetKeyDown(KeyCode.Space) && !isAirborn)
        {
            skateboardBody.velocity = new Vector2(skateboardBody.velocity.x, jumpForce);
            isAirborn = true;
            PlaySound("Jump");
        }

        if (!WinCanvas.activeSelf && !LoseCanvas.activeSelf) //Updates our camera position to autoscroll, so long as we have not won or lost yet.
        {
            
            cam.transform.position += Vector3.right * cameraSpeed * Time.deltaTime;           
        }

        //Trick is allowed to be performed
        if (trickTimer == 0 && isAirborn)
        {
            if (Input.GetKeyDown(KeyCode.A)) //Ollie
            {
                trick = "Ollie";
                PerformTrick(1, 0.1f);
            }
            else if (Input.GetKeyDown(KeyCode.B)) //Backside Heelflip
            {
                trick = "BacksideHeelflip";
                PerformTrick(3, 0.3f);        
            }
            else if (Input.GetKeyDown(KeyCode.C)) //50-50 Grind
            {
                trick = "5050Grind";
                PerformTrick(2, 0.0f);              
            }
            else if (Input.GetKeyDown(KeyCode.D)) //Feeble Grind
            {
                trick = "FeebleGrind";
                PerformTrick(3, 0.3f);
            }
            else if (Input.GetKeyDown(KeyCode.E)) //Backflip
            {
                trick = "Backflip";
                PerformTrick(4, 0.5f);
            }
            else if (Input.GetKeyDown(KeyCode.F)) //Helicopter
            {
                trick = "Helicopter";
                PerformTrick(6, 0.6f);  
            }
        }    
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Collision detection methods
        if (collision.gameObject.name == "Rail")
        {
            if (trick != "5050Grind" || trick != "FeebleGrind")
            {
                ShowLoseScreen();
            }
        }

        if (collision.gameObject.name == "Pedestrian")
        {
            ShowLoseScreen();
        }

        if (collision.gameObject.name == "Ground")
        {
            isAirborn = false;
            if (trickTimer <= 0) 
            {
                ResetTrickState();
            }
        }

        if (collision.gameObject.name == "Finish Line")
        {
            ShowWinScreen();
        }
    }

    void ResetTrickState()
    {
        ChangeSprite("Skater");
        trick = "None";
        trickTimer = 0;
    }

    //Rolls our points and sets our tricktime up.
    void PerformTrick(int points, float failureChance) 
    {
        trickTimer = trickDuration;
        float randValue = Random.value;
        if (randValue < failureChance) // Fails!
        {
            ScoreKeeper.SubtractPoints(points);
            ChangeSprite("FailedTrick");
            PlaySound("TrickBad");
        }
        else //Succeeds!
        {
            ScoreKeeper.AddPoints(points);
            ChangeSprite(trick);
            PlaySound("TrickGood");
        }

    }

    void PlaySound(string soundName)
    {
        AudioClip clip = Resources.Load<AudioClip>("Sounds/" + soundName);

        if (clip != null)
        {
            soundEffects.clip = clip;
            soundEffects.Play();
        }
        else
        {
            Debug.LogError("Sound not found: " + soundName);
        }
    }

    public void ChangeSprite(string spriteName)
    {
        Sprite newSprite = Resources.Load<Sprite>("Sprites/" + spriteName);

        if (newSprite != null)
        {
            spriteRenderer.sprite = newSprite;
        }
        else
        {
            Debug.LogError("Sprite not found: " + spriteName);
        }
    }

    void ShowLoseScreen()
    {
        PlaySound("TargetHit");
        LoseCanvas.SetActive(true);
    }

    void ShowWinScreen()
    {
        PlaySound("TrickGood");
        WinCanvas.SetActive(true);
    }
}
