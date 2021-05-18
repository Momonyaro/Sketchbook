using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Config;
using PathCreation;
using UnityEngine.SceneManagement;

namespace Movement
{
    public class PlayerHurt : MonoBehaviour
    {
        // Having the max HP be scene-dependent is kinda whack, but eh it's probably fine, we're not planning on ever changing the max HP of the player anyways
        [HideInInspector]
        public int maximumHP = 12;
        [Tooltip("The canvas with the transition object that gets spawned when a scene transition is triggered (i.e. the white fade-in)")]
        public GameObject deathScreenCanvas;
        [HideInInspector]
        static public int currentHP = 12; //endast static public int just nu för att det blir en enkel lösning tills speltestningen

        float knockbackSpeed = 0.0f;
        int direction = 1;

        PlayerController playerController;
        Rigidbody rb;
        SplineWalker splineWalker;
        HPUI hpUI;

        bool isHurt;
        bool invulnerable; //Used to keep the player from getting hurt while they're already hurt. Basically this means that if hurt, the player can't be hurt again. If they were already hurt, of course. As in, the player can't get hurt while they are already hurt.
        //To put it more simply, the player can only get hurt if they are not currently hurt. This gives our depressed players an advantage, to make them feel better. Basically this means that the player CANNOT be hurt by an enemy or anything else as long as they
        //are already hurt, you feel me? It might make more sense if you think of it like this: I was very bored. Also please don't cancel me on twitter dot com if the depressed joke was made in poor taste, which it probably was. I'm bored, yo. That's not
        //an excuse for being rude though, I am very sorry. I've made a severe lapse in my judgement, and I don't expect to be forgiven. I'm simply here to apologize. I'm ashamed of myself. I’m disappointed in myself. And I promise to be better. I will be better.
        //Thank you.

        // Start is called before the first frame update
        void Start()
        {
            if (currentHP <= 0)
                currentHP = maximumHP;

            playerController = GetComponent<PlayerController>();
            rb = GetComponent<Rigidbody>();
            splineWalker = GetComponent<SplineWalker>();
            if (FindObjectOfType<HPUI>() != null)
            {
                hpUI = FindObjectOfType<HPUI>();
                hpUI.UpdateUI(currentHP, maximumHP, false);
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (isHurt)
                MoveAlongSplineHor(direction);
        }

        public void MoveAlongSplineHor(float horSpeed) // Detta kinda dumt, borde använda mig av samma del som finns i PlayerController
        {
            //Varför ska vi röra oss?
            if (horSpeed == 0) return;

            horSpeed *= Time.deltaTime * knockbackSpeed;

            var position = rb.position;

            //Animations Go Here

            PathCreator currentSpline = splineWalker.currentSpline;
            float splineDist = currentSpline.path.GetClosestDistanceAlongPath(position);
            Vector3 splinePos = currentSpline.path.GetPointAtDistance(splineDist - horSpeed, EndOfPathInstruction.Stop);

            //This is garbage but ok for testing
            if (!MapSettings.Instance.configScriptable.lockYToSpline) splinePos.y = position.y;

            rb.MovePosition(splinePos);
        }

        public void GettingHurt(int damage, int direction, float horKnbck, float verKnbck)
        {
            if (!invulnerable)
            {
                knockbackSpeed = horKnbck;
                this.direction = direction;
                playerController.BounceUsingForce(verKnbck);
                currentHP -= damage;
                StartCoroutine(HurtKnockback());
                hpUI.UpdateUI(currentHP, maximumHP, true);
            }
        }

        IEnumerator HurtKnockback()
        {
            isHurt = true;
            invulnerable = true;
            playerController.hurtSpeedMultiplier = 0.0f;
            yield return new WaitForSeconds(0.5f);
            isHurt = false;
            if (currentHP <= 0)
                StartCoroutine(Die());
            else
                playerController.hurtSpeedMultiplier = 1.0f;
            yield return new WaitForSeconds(0.5f);
            invulnerable = false;
        }

        public IEnumerator Die() // so sad
        {
            if (deathScreenCanvas != null)
                Instantiate(deathScreenCanvas, transform.position, transform.rotation);

            playerController.hurtSpeedMultiplier = 0.0f;
            yield return new WaitForSeconds(0.5f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}