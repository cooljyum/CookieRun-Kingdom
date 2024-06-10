using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductUI : MonoBehaviour
{
    public BuildingData _buildingData;
    public int _productID;

    //public 

    private void Awake()
    {
        ProductInfo productInfo = _buildingData.Products[_productID];

        SetData(productInfo);
    }

    private void SetData(ProductInfo productInfo)
    {


        if(productInfo.IsMaterial)
        {

        }
    }
}
