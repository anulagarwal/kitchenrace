using System;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.iOS;

public class AppCentral
{

    public static void TrackInstall()
    {
        String trackUrl = $"https://vnc412s287.execute-api.us-east-1.amazonaws.com/default/unity-tracker?v=3&action=start&appid={Application.identifier}&installID={AppCentral.GetInstallID()}&idfa={Device.advertisingIdentifier}&idfv={Device.vendorIdentifier}";
        UnityWebRequest.Get(trackUrl).SendWebRequest();
    }

    public static Purchaser purchaser;
    public static void ShowPaywall(MonoBehaviour m, String productID)
    {
        AppCentral.TrackInstall();
        Purchaser.kProductNameAppleSubscription = productID;
        Purchaser.kProductIDSubscription = productID;
        Debug.Log("Init product " + productID);
        AppCentral.purchaser = m.gameObject.AddComponent<Purchaser>() as Purchaser;
        AppCentral.purchaser.InitializePurchasing();
    }

    static String GetInstallID()
    {
        String installIdKey = "install_id_key";
        String installID = PlayerPrefs.GetString(installIdKey, "no_guid");
        if (installID == "no_guid")
        {
            installID = System.Guid.NewGuid().ToString();
            PlayerPrefs.SetString(installIdKey, installID);
        }
        return installID;
    }

    public static void ShowPaywall()
    {
        if (AppCentral.IsUserSubscribed())
        {
            Debug.Log("User already subscribed, not showing paywall");
            return;
        }
        Debug.Log("Showing paywall");
        AppCentral.purchaser.windowOpen = true;
    }

    public static Boolean IsUserSubscribed()
    {
        return 1 == PlayerPrefs.GetInt("purchased_subscription");
    }

    //purchasing
    // Deriving the Purchaser class from IStoreListener enables it to receive messages from Unity Purchasing.
    public class Purchaser : MonoBehaviour, IStoreListener
    {

        private GUIStyle currentStyle = null;
        private int windowWidth;
        private int windowHeight;
        private Rect windowRect;
        public Boolean windowOpen = false;
        private String localizedPriceString = "Price";
        private Boolean priceReceived = false;
        private String localizedTitle = "Full Game";
        private String localizedDescription = "Access all levels";

        void OnGUI()
        {
            if (currentStyle == null || currentStyle.normal.background == null)
            {
                currentStyle = new GUIStyle(GUI.skin.box);
                currentStyle.normal.background = MakeTex(2, 2, new Color(0.13f, 0.41f, 0.52f, 0.9f));
            }
            if (windowOpen)
            {
                windowWidth = 600;
                windowHeight = 600;
                windowRect = new Rect((Screen.width - windowWidth) / 2, (Screen.height - windowHeight) / 2, windowWidth, windowHeight);
                GUI.ModalWindow(0, windowRect, PopWindow, "", currentStyle);
            }
        }

        void PopWindow(int windowID)
        {
            if (priceReceived)
            {
                PaywallWindow(windowID);
            }
            else
            {
                ErrorWindow(windowID);
            }
        }

        void PaywallWindow(int windowID)
        {
            GUIStyle myButtonStyle = new GUIStyle(GUI.skin.button);
            myButtonStyle.fontSize = 35;

            Color color = Color.white;
            color.a = 0.0f;
            GUI.backgroundColor = color;
            if (GUI.Button(new Rect(10, 10, 40, 40), "X", myButtonStyle))
            {
                windowOpen = false;
            }

            GUIStyle myLabelStyle = new GUIStyle(GUI.skin.label);
            myLabelStyle.fontSize = 35;
            myLabelStyle.alignment = TextAnchor.UpperCenter;
            GUI.Label(new Rect(0, 100, windowWidth, 300), localizedTitle + "\n" + localizedDescription + "\n\nJust " + localizedPriceString + "/month", myLabelStyle);

            int extraButtonsWidth = 200;
            if (GUI.Button(new Rect(50, windowHeight - 100, extraButtonsWidth, 50), "Terms", myButtonStyle))
            {
                print("Terms");
                Application.OpenURL("https://www.app-central.com/terms");
            }

            if (GUI.Button(new Rect(windowWidth - extraButtonsWidth - 50, windowHeight - 100, extraButtonsWidth, 50), "Restore", myButtonStyle))
            {
                print("Restore");
                AppCentral.purchaser.RestorePurchases();
                windowOpen = false;
            }

            color.a = 1.0f;
            GUI.backgroundColor = color;
            GUIStyle purchaseButtonStyle = new GUIStyle(GUI.skin.button);
            purchaseButtonStyle.fontSize = 50;

            int buttonWidth = 400;
            if (GUI.Button(new Rect(windowWidth / 2 - buttonWidth / 2, 300, buttonWidth, 100), "Subscribe", purchaseButtonStyle))
            {
                print("Purchasing!");
                AppCentral.purchaser.BuySubscription();
                windowOpen = false;
            }
        }
        void ErrorWindow(int windowID)
        {
            GUIStyle myButtonStyle = new GUIStyle(GUI.skin.button);
            myButtonStyle.fontSize = 35;

            Color color = Color.white;
            color.a = 0.0f;
            GUI.backgroundColor = color;
            if (GUI.Button(new Rect(10, 10, 40, 40), "X", myButtonStyle))
            {
                windowOpen = false;
            }

            GUIStyle myLabelStyle = new GUIStyle(GUI.skin.label);
            myLabelStyle.fontSize = 35;
            myLabelStyle.alignment = TextAnchor.UpperCenter;
            GUI.Label(new Rect(0, 100, windowWidth, 300), "Something went wrong :(\nMake sure your device can make purchases and try again.", myLabelStyle);

            color.a = 1.0f;
            GUI.backgroundColor = color;
            GUIStyle purchaseButtonStyle = new GUIStyle(GUI.skin.button);
            purchaseButtonStyle.fontSize = 50;

            int buttonWidth = 400;
            if (GUI.Button(new Rect(windowWidth / 2 - buttonWidth / 2, 300, buttonWidth, 100), "OK", purchaseButtonStyle))
            {
                windowOpen = false;
            }
        }

        private Texture2D MakeTex(int width, int height, Color col)
        {
            Color[] pix = new Color[width * height];
            for (int i = 0; i < pix.Length; ++i)
            {
                pix[i] = col;
            }
            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();
            return result;
        }

        private static IStoreController m_StoreController;          // The Unity Purchasing system.
        private static IExtensionProvider m_StoreExtensionProvider; // The store-specific Purchasing subsystems.

        // Product identifiers for all products capable of being purchased: 
        // "convenience" general identifiers for use with Purchasing, and their store-specific identifier 
        // counterparts for use with and outside of Unity Purchasing. Define store-specific identifiers 
        // also on each platform's publisher dashboard (iTunes Connect, Google Play Developer Console, etc.)

        // General product identifiers for the consumable, non-consumable, and subscription products.
        // Use these handles in the code to reference which product to purchase. Also use these values 
        // when defining the Product Identifiers on the store. Except, for illustration purposes, the 
        // kProductIDSubscription - it has custom Apple and Google identifiers. We declare their store-
        // specific mapping to Unity Purchasing's AddProduct, below.
        public static string kProductIDConsumable = "consumable";
        public static string kProductIDNonConsumable = "nonconsumable";
        public static string kProductIDSubscription;

        // Apple App Store-specific product identifier for the subscription product.
        public static string kProductNameAppleSubscription;
        // Google Play Store-specific product identifier subscription product.
        private static string kProductNameGooglePlaySubscription = "com.unity3d.subscription.original";

        void Start()
        {
            // If we haven't set up the Unity Purchasing reference
            if (m_StoreController == null)
            {
                // Begin to configure our connection to Purchasing
                InitializePurchasing();
            }
        }

        public void InitializePurchasing()
        {
            // If we have already connected to Purchasing ...
            if (IsInitialized())
            {
                // ... we are done here.
                return;
            }

            // Create a builder, first passing in a suite of Unity provided stores.
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

            // Add a product to sell / restore by way of its identifier, associating the general identifier
            // with its store-specific identifiers.
            builder.AddProduct(kProductIDConsumable, ProductType.Consumable);
            // Continue adding the non-consumable product.
            builder.AddProduct(kProductIDNonConsumable, ProductType.NonConsumable);
            // And finish adding the subscription product. Notice this uses store-specific IDs, illustrating
            // if the Product ID was configured differently between Apple and Google stores. Also note that
            // one uses the general kProductIDSubscription handle inside the game - the store-specific IDs 
            // must only be referenced here. 
            builder.AddProduct(kProductIDSubscription, ProductType.Subscription, new IDs(){
                { kProductNameAppleSubscription, AppleAppStore.Name },
                { kProductNameGooglePlaySubscription, GooglePlay.Name },
            });

            // Kick off the remainder of the set-up with an asynchrounous call, passing the configuration 
            // and this class' instance. Expect a response either in OnInitialized or OnInitializeFailed.
            UnityPurchasing.Initialize(this, builder);
        }


        private bool IsInitialized()
        {
            // Only say we are initialized if both the Purchasing references are set.
            return m_StoreController != null && m_StoreExtensionProvider != null;
        }


        public void BuyConsumable()
        {
            // Buy the consumable product using its general identifier. Expect a response either 
            // through ProcessPurchase or OnPurchaseFailed asynchronously.
            BuyProductID(kProductIDConsumable);
        }


        public void BuyNonConsumable()
        {
            // Buy the non-consumable product using its general identifier. Expect a response either 
            // through ProcessPurchase or OnPurchaseFailed asynchronously.
            BuyProductID(kProductIDNonConsumable);
        }


        public void BuySubscription()
        {
            // Buy the subscription product using its the general identifier. Expect a response either 
            // through ProcessPurchase or OnPurchaseFailed asynchronously.
            // Notice how we use the general product identifier in spite of this ID being mapped to
            // custom store-specific identifiers above.
            BuyProductID(kProductIDSubscription);
        }


        void BuyProductID(string productId)
        {
            // If Purchasing has been initialized ...
            if (IsInitialized())
            {
                // ... look up the Product reference with the general product identifier and the Purchasing 
                // system's products collection.
                Product product = m_StoreController.products.WithID(productId);

                // If the look up found a product for this device's store and that product is ready to be sold ... 
                if (product != null && product.availableToPurchase)
                {
                    Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                    // ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed 
                    // asynchronously.
                    m_StoreController.InitiatePurchase(product);
                }
                // Otherwise ...
                else
                {
                    // ... report the product look-up failure situation  
                    Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
                }
            }
            // Otherwise ...
            else
            {
                // ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or 
                // retrying initiailization.
                Debug.Log("BuyProductID FAIL. Not initialized.");
            }
        }


        // Restore purchases previously made by this customer. Some platforms automatically restore purchases, like Google. 
        // Apple currently requires explicit purchase restoration for IAP, conditionally displaying a password prompt.
        public void RestorePurchases()
        {
            // If Purchasing has not yet been set up ...
            if (!IsInitialized())
            {
                // ... report the situation and stop restoring. Consider either waiting longer, or retrying initialization.
                Debug.Log("RestorePurchases FAIL. Not initialized.");
                return;
            }

            // If we are running on an Apple device ... 
            if (Application.platform == RuntimePlatform.IPhonePlayer ||
                Application.platform == RuntimePlatform.OSXPlayer)
            {
                // ... begin restoring purchases
                Debug.Log("RestorePurchases started ...");

                // Fetch the Apple store-specific subsystem.
                var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
                // Begin the asynchronous process of restoring purchases. Expect a confirmation response in 
                // the Action<bool> below, and ProcessPurchase if there are previously purchased products to restore.
                apple.RestoreTransactions((result) =>
                {
                    // The first phase of restoration. If no more responses are received on ProcessPurchase then 
                    // no purchases are available to be restored.
                    Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
                });
            }
            // Otherwise ...
            else
            {
                // We are not running on an Apple device. No work is necessary to restore purchases.
                Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
            }
        }


        //  
        // --- IStoreListener
        //

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            // Purchasing has succeeded initializing. Collect our Purchasing references.
            Debug.Log("OnInitialized: PASS");

            // Overall Purchasing system, configured with products for this application.
            m_StoreController = controller;
            // Store specific subsystem, for accessing device-specific store features.
            m_StoreExtensionProvider = extensions;

            Debug.Log("Product: " + Purchaser.kProductNameAppleSubscription);
            Product product = m_StoreController.products.WithID(Purchaser.kProductNameAppleSubscription);

            Debug.Log("isoCurrencyCode: " + product.metadata.isoCurrencyCode);
            Debug.Log("localizedDescription: " + product.metadata.localizedDescription);
            Debug.Log("localizedPrice: " + product.metadata.localizedPrice);
            Debug.Log("localizedPriceString: " + product.metadata.localizedPriceString);
            Debug.Log("localizedTitle: " + product.metadata.localizedTitle);

            if (product.metadata.localizedTitle.Length > 0)
            {
                localizedTitle = product.metadata.localizedTitle;
            }
            if (product.metadata.localizedDescription.Length > 0)
            {
                localizedDescription = product.metadata.localizedDescription;
            }
            if (product.metadata.localizedPriceString != null)
            {
                priceReceived = true;
                localizedPriceString = product.metadata.localizedPriceString;
            }

            AppCentral.ShowPaywall();
        }


        public void OnInitializeFailed(InitializationFailureReason error)
        {
            // Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
            Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
        }


        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
        {
            // A consumable product has been purchased by this user.
            if (String.Equals(args.purchasedProduct.definition.id, kProductIDConsumable, StringComparison.Ordinal))
            {
                Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
                // The consumable item has been successfully purchased, add 100 coins to the player's in-game score.
            }
            // Or ... a non-consumable product has been purchased by this user.
            else if (String.Equals(args.purchasedProduct.definition.id, kProductIDNonConsumable, StringComparison.Ordinal))
            {
                Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
                // TODO: The non-consumable item has been successfully purchased, grant this item to the player.
            }
            // Or ... a subscription product has been purchased by this user.
            else if (String.Equals(args.purchasedProduct.definition.id, kProductIDSubscription, StringComparison.Ordinal))
            {
                Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
                //The subscription item has been successfully purchased, grant this to the player.
                PlayerPrefs.SetInt("purchased_subscription", 1);
            }
            // Or ... an unknown product has been purchased by this user. Fill in additional products here....
            else
            {
                Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
            }

            // Return a flag indicating whether this product has completely been received, or if the application needs 
            // to be reminded of this purchase at next app launch. Use PurchaseProcessingResult.Pending when still 
            // saving purchased products to the cloud, and when that save is delayed. 
            return PurchaseProcessingResult.Complete;
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            // A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing 
            // this reason with the user to guide their troubleshooting actions.
            Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
        }
    }
}