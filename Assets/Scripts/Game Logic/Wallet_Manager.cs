using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Solana.Unity.SDK;
using System.Collections;
using System.Threading.Tasks;

public class Wallet_Manager : MonoBehaviour
{
    [Header("UI References")]
    public Button loginButton;
    public TMP_Text publicKeyText;
    public TMP_Text balanceText;

    [Header("Editor Test Mode")]
    public bool editorTestMode = true;
    public string testPublicKey = "HzUEcLTkpKVmk3eL25HYJrzT6nZSMDAvyo22SXuXRg5y";
    public double testBalance = 10;

    private string lastPublicKey = null;
    private double lastBalance = -1;

    private void Start()
    {
        loginButton.onClick.AddListener(OnLoginButtonClicked);
        publicKeyText.text = "Not logged in";
        balanceText.text = "Balance: 0";

        StartCoroutine(PollWallet());
    }

    private void Update()
    {
        // Press Space to simulate balance increase in DebugMode
        if (editorTestMode && Input.GetKeyDown(KeyCode.Space))
        {
            testBalance += 1;
            Debug.Log($"Editor: Balance increased! Current: {testBalance}");
        }
    }

    private void OnLoginButtonClicked()
    {
        if (editorTestMode)
        {
            Debug.Log("Editor test mode: simulating wallet login");
            loginButton.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Opening Wallet Adapter...");
            Web3.Instance.LoginWalletAdapter();

            var wallet = Web3.Instance.WalletBase;
            if (wallet == null)
            {
                Debug.Log("No wallet adapter found in Web3 instance!");
            }
            else
            {
                Debug.Log($"Wallet adapter type: {wallet.GetType().Name}");
                loginButton.gameObject.SetActive(false);
            }
        }
    }

    private IEnumerator PollWallet()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);

            string currentKey = null;
            double currentBalance = 0;

#if UNITY_EDITOR
            if (editorTestMode)
            {
                currentKey = testPublicKey;
                currentBalance = testBalance;
            }
#endif
            if (!editorTestMode)
            {
                var wallet = Web3.Instance.WalletBase;
                if (wallet != null && wallet.Account != null && wallet.Account.PublicKey != null)
                {
                    currentKey = wallet.Account.PublicKey;
                    // Async GetBalance
                    Task<double> balanceTask = wallet.GetBalance();
                    yield return new WaitUntil(() => balanceTask.IsCompleted);
                    currentBalance = balanceTask.Result;
                }
            }

            // Update public key UI
            if (currentKey != lastPublicKey)
            {
                lastPublicKey = currentKey;
                publicKeyText.text = string.IsNullOrEmpty(currentKey)
                    ? "Not logged in"
                    : $"{currentKey}";
                Debug.Log(currentKey != null ? $"✅ Logged in: {currentKey}" : "🚪 Wallet disconnected");
            }

            // Update balance UI and spawn pet if balance increased
            if (currentBalance != lastBalance)
            {
                if (lastBalance >= 0 && currentBalance > lastBalance)
                {
                    Debug.Log($"Balance increased: {lastBalance} → {currentBalance}");
                    Spawner_Manager.Instance.SpawnRandomPet();
                }

                lastBalance = currentBalance;
                balanceText.text = $"{currentBalance} SOL";
            }
        }
    }
}
