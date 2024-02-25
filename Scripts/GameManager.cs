using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Ghost[] ghosts;
    public Pacman pacman;
    public Transform pellets;

    public int score { get; private set; }
    public int lives { get; private set; }
    public int ghostMultiplier { get; private set; } = 1;

    private void Start()
    {
        NewGame();
    }

    private void Update()
    {
        if (this.lives <= 0 && Input.anyKeyDown) {
            NewGame();
        }
    }

    private void NewGame()
    {
        setScore(0);
        SetLives(3);
        NewRound();

    }

    private void NewRound()
    {
        foreach (Transform pellet in this.pellets) {
            pellet.gameObject.SetActive(true);
        }

        ResetState();
    }

    private void ResetState()
    {
        ResetGhostMultiplier();
          for (int i = 0; i < this.ghosts.Length; i++) {
            this.ghosts[i].ResetState();
        }
        
        this.pacman.ResetState();
    }

    private void GameOver()
    {
         for (int i = 0; i < this.ghosts.Length; i++) {
            this.ghosts[i].gameObject.SetActive(false);
        }
        
        this.pacman.gameObject.SetActive(false);
    }

    private void setScore(int score)
    {
        this.score = score;
    }

    private void SetLives(int lives)
    {
        this.lives = lives;
    }

    public void GhostEaten(Ghost ghosts)
    {
        int points = ghosts.points * this.ghostMultiplier;
        setScore(this.score + points);
        this.ghostMultiplier++;
    }

    public void PacmanEaten()
    {
        this.pacman.gameObject.SetActive(false);

        SetLives(this.lives - 1);

        if (this.lives > 0) {
            Invoke(nameof(ResetState), 3.0f);
        } else {
            GameOver();
        }
    }

    public void PelletEaten(Pellet pellet)
    {
        pellet.gameObject.SetActive(false);
        setScore(this.score + pellet.points);
        if (!HasRemainingPellets())
        {
            this.pacman.gameObject.SetActive(false);
            Invoke(nameof(NewRound), 3.0f);
        }
    }

    public void PowerPelletEaten(PowerPellet pellet)
    {
        Invoke(nameof(ResetGhostMultiplier), pellet.duration);
        CancelInvoke();
        PelletEaten(pellet);
    }

    private bool HasRemainingPellets()
    {
        foreach (Transform pellet in this.pellets)
        {
            if (pellet.gameObject.activeSelf) {
                return true;
            }
        }

        return false;
    }

    private void ResetGhostMultiplier()
    {
        this.ghostMultiplier = 1;
    }
}
