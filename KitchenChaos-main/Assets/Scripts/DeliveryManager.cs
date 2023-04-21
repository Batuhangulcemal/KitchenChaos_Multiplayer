using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted;
    public event EventHandler OnRecipeSuccess;
    public event EventHandler OnRecipeFailed;
    public static DeliveryManager Instance { get; private set; }

    [SerializeField] private RecipeListSO recipeListSO;

    private List<RecipeSO> waitingRecipeSOList;

    private float spawnRecipeTimer;
    private float spawnRecipeTimerMax = 4f;
    private int waitingRecipesMax = 4;
    private int successfulRecipesAmount;

    private void Awake()
    {
        Instance = this;
        waitingRecipeSOList = new List<RecipeSO>();
        successfulRecipesAmount = 0;
    }

    private void Update()
    {
        spawnRecipeTimer -= Time.deltaTime;
        if(spawnRecipeTimer < 0) 
        {
            spawnRecipeTimer = spawnRecipeTimerMax;
            if(KitchenGameManager.Instance.IsGamePlaying() && waitingRecipesMax > waitingRecipeSOList.Count)
            {
                RecipeSO waitingRecipeSo = recipeListSO.recipeSOList[UnityEngine.Random.Range(0, recipeListSO.recipeSOList.Count)];
                waitingRecipeSOList.Add(waitingRecipeSo);
                OnRecipeSpawned?.Invoke(this, EventArgs.Empty);

            }

        }
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        for (int i = 0; i < waitingRecipeSOList.Count; i++)
        {
            RecipeSO waitingRecipeSO = waitingRecipeSOList[i];

            if (waitingRecipeSO.kitchenObjectSOList.Count == plateKitchenObject.GetKitchenObjectSOList().Count)
            {
                //has the same number of ingredients
                bool plateContentsMatchesRecipe = true;
                foreach(KitchenObjectSO recipeKitchenObjectSO in waitingRecipeSO.kitchenObjectSOList)
                {
                    bool ingredientFound = false;

                    foreach (KitchenObjectSO plateKitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList())
                    {
                        if(plateKitchenObjectSO == recipeKitchenObjectSO)
                        {
                            //Ingredient Match
                            ingredientFound = true;
                        }
                    }
                    
                    if(!ingredientFound)
                    {
                        plateContentsMatchesRecipe = false;
                    }
                }

                if (plateContentsMatchesRecipe)
                {
                    //Recipe match!

                    waitingRecipeSOList.RemoveAt(i);

                    OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
                    OnRecipeSuccess?.Invoke(this, EventArgs.Empty);

                    successfulRecipesAmount++;

                    return;
                }
            }
        }

        OnRecipeFailed?.Invoke(this, EventArgs.Empty);
        //No matches
        return;
    }

    public List<RecipeSO> GetWaitingRecipeSoList()
    {
        return waitingRecipeSOList;
    }

    public int GetSuccessfulRecipeAmount()
    {
        return successfulRecipesAmount;
    }
}
