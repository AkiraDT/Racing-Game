using UnityEngine;
using UnityEngine.UI;
#if UNITY_ADS
using UnityEngine.Advertisements;
#endif
using System;

namespace TurnTheGameOn{

	public class UnityAds : MonoBehaviour {

		[System.Serializable]
		public class AdRewards {
			public enum AdCooldown{ None = 0, OneMinute = 60, TwoMinutes = 120, ThreeMinutes = 180, FourMinutes = 240, FiveMinutes = 300,
				TenMinutes = 600, FifteenMinutes = 900, TwentyMinutes = 1200, ThirtyMinutes = 1800, 
				OneHour = 3600, TwoHours = 7200, ThreeHours = 10800, FourHours = 14400, FiveHours = 18000, SixHours = 21600, TwelveHours = 43200, 
				OneDay = 86400, TwoDays = 172800, ThreeDays = 259200, FourDays = 345600, FiveDays = 432000, SixDays = 518400, 
				OneWeek = 604800, TwoWeeks = 1209600, ThreeWeeks = 1814400, FourWeeks = 2419200}

			public int rewardCurrency;

			public GameObject rewardAd;
			public GameObject rewardMessage;
			public int cooldownSeconds;
			public AdCooldown adCooldown;
			public string lastAdTime;
			public Text rewardText;
		}

		public GameObject[] disableForAds;
		public bool showDebugs;
		public AdRewards adRewards;
		public RG_GarageManager garageManager;

#if UNITY_ADS

		void OnEnable(){
			adRewards.cooldownSeconds = (int)adRewards.adCooldown;

			adRewards.lastAdTime = PlayerPrefs.GetString ("lastAdTime");
			if (adRewards.lastAdTime == "") {
				adRewards.rewardAd.SetActive (true);
			} else {
				DateTime oldDate = Convert.ToDateTime (adRewards.lastAdTime);
				DateTime newDate = System.DateTime.Now;
				TimeSpan difference = newDate.Subtract (oldDate);
				if(showDebugs)
					Debug.Log ("Rewarded Video Ad was shown " + ((Double)difference.TotalSeconds).ToString("F0") + " seconds ago");
				if((Double)difference.TotalSeconds > adRewards.cooldownSeconds){
				}else {
					adRewards.rewardAd.SetActive (false);
				}
			}
		}

		void Update(){
			///
			/// If an ad button is not active in the scene, then we shoud check that ad button's lastAdTime to see when it will be available
			///
			if(!adRewards.rewardAd.activeInHierarchy){
				HandleAd1 ();
			}
		}

		///
		/// 
		///
		void HandleAd1 (){
			if (adRewards.lastAdTime != "") {
				DateTime oldDate = Convert.ToDateTime (adRewards.lastAdTime);
				DateTime newDate = System.DateTime.Now;
				TimeSpan difference = newDate.Subtract (oldDate);
				if ((Double)difference.TotalSeconds >= adRewards.cooldownSeconds) {
					adRewards.rewardAd.SetActive (true);
				}
			}
		}


		[ContextMenu("ShowRewardedAd C1")]
		public void ShowRewardedAdC1(){
			if (Advertisement.IsReady("rewardedVideo")){
				for(int i = 0; i < disableForAds.Length; i++){
					disableForAds [i].SetActive (false);
				}
				var options = new ShowOptions { resultCallback = HandleShowResultC1 };
				Advertisement.Show("rewardedVideo", options);
			}
		}

		private void HandleShowResultC1(ShowResult result){
			switch (result)	{
			case ShowResult.Finished:
				if(showDebugs)
					Debug.Log ("The ad was successfully shown.");
				adRewards.rewardAd.SetActive (false);
				adRewards.lastAdTime = System.DateTime.Now.ToString();
				PlayerPrefs.SetString ("lastAdTime", adRewards.lastAdTime);
				for(int i = 0; i < disableForAds.Length; i++){
					disableForAds [i].SetActive (true);
				}
				int currency = PlayerPrefs.GetInt ("Currency");
				currency += adRewards.rewardCurrency;
				PlayerPrefs.SetInt ("Currency", currency);
				garageManager.UpdateCurrency ();
				adRewards.rewardText.text = adRewards.rewardCurrency.ToString ("N0");
				adRewards.rewardMessage.SetActive (true);
				break;
			case ShowResult.Skipped:
				if(showDebugs)
					Debug.Log("The ad was skipped before reaching the end.");
				break;
			case ShowResult.Failed:
				if(showDebugs)
					Debug.LogError("The ad failed to be shown.");
				break;
			}
		}
#else
		//Ads are not active so the button will be disabled.
		void Awake(){
			adRewards.rewardAd.SetActive (false);
		}
#endif


	}
}