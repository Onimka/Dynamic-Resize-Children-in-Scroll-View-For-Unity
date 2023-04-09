using System.Collections.Generic;
using UnityEngine;

public class ScrollResizer : MonoBehaviour
{
    [SerializeField] private RectTransform centralPoint;
    [SerializeField] private bool canSelectObj = false;
    [SerializeField] private VariantResize variantResize = VariantResize.Variant_1;
    [SerializeField, Range(0f, 1f)] private float minScale = 0.1f;
    [SerializeField, Range(1f, 10f)] private float distanceToResize = 10f;
    [SerializeField, Range(1f, 10f)] private float speedResize = 3f;
    [SerializeField] private float distanceToSelect = 0.7f;

    [Header("For Variant 2:")]
    [SerializeField] private float limitedAngle = 90f;

    [SerializeField] private ExampleAdapterCardScrollToView adapterCardToScroll;  // Delete it, This is for EXAMPLE                                                              

    private RectTransform[] cards;
    private RectTransform selectedCard;

    private void Start()
    {
        cards = GetCards();
    }

    // Add new Cards
    #region 
    private void OnTransformChildrenChanged()
    {
        Debug.Log("isAdd");
        cards = GetCards();
    }
    private RectTransform[] GetCards()
    {
        List<RectTransform> filteredCards = new List<RectTransform>();
        foreach (RectTransform card in GetComponentsInChildren<RectTransform>())
        {
            if (card.gameObject != this.gameObject)
            {
                filteredCards.Add(card);
            }
        }
        return filteredCards.ToArray();
    }
    #endregion 

    private void Update()
    {
        ChangeScale();
    }

    public void ChangeScale()
    {
        foreach (RectTransform card in cards)
        {
            float distance = Vector2.Distance(centralPoint.position, card.position);
            float scale;

            if (distance > distanceToSelect)
            {
                scale = Mathf.Clamp(distance / distanceToResize, 0f, 1f - minScale);

                if (variantResize == VariantResize.Variant_2)
                {
                    float rotateAmount = Mathf.Clamp(distance / distanceToResize, 0f, 0.5f);
                    float angle = Mathf.Lerp(0f, limitedAngle, rotateAmount);
                    angle = (card.position.x < centralPoint.position.x) ? -angle : angle;
                    card.rotation = Quaternion.Euler(0f, angle, 0f);
                }
            }
            else
            {
                scale = 0f;
                if (variantResize == VariantResize.Variant_2)
                {
                    card.rotation = Quaternion.identity;
                }
            }

            float currentScale = 1 - card.localScale.x;
            float newScale = Mathf.Lerp(currentScale, scale, Time.deltaTime * speedResize);
            card.localScale = new Vector2(1 - newScale, 1 - newScale);

            if (canSelectObj)
            {
                SelectCard(distance, card);
            }
        }
    }

    private void SelectCard(float distance, RectTransform card)
    {
        if (distance < distanceToSelect && card != selectedCard)
        {
            if (selectedCard != null)
            {
                selectedCard.GetComponent<InteractableCard>().Selectable(false);
            }

            selectedCard = card;
            InteractableCard newCard = selectedCard.GetComponent<InteractableCard>();
            newCard.Selectable(true);

            // Delete it, This is for EXAMPLE   
            adapterCardToScroll.SetCharacterParametres(newCard.Name, newCard.Strength, newCard.Description);
            //
        }
    }
}

public enum VariantResize
{
    Variant_1,
    Variant_2
}
