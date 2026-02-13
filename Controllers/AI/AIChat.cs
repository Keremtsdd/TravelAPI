using Microsoft.AspNetCore.Mvc;
using TravelAPI.Models;

namespace TravelAPI.Controllers.AI
{
    [ApiController]
    [Route("api/ai/chat")]
    public class AIChatController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _httpClient;

        public AIChatController(IConfiguration config, IHttpClientFactory httpClientFactory)
        {
            _config = config;
            _httpClient = httpClientFactory.CreateClient();
        }

        [HttpPost("ask")]
        public async Task<IActionResult> AskGemini([FromBody] ChatRequest request)
        {
            var apiKey = _config["Gemini:ApiKey"];

            // URL'yi 'v1beta' ve model ismini tam yol (models/gemini-1.5-flash) olacak şekilde güncelleyin
            var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent?key={apiKey}";

            #region AI Promt
            string systemPrompt = "Sen 'Seyahat Asistanı' rolünde, profesyonel, nazik ve çözüm odaklı bir yapay zekasın. " +
                      "Görevin SADECE gezilecek yerler, vize durumları, konaklama, ulaşım ve rota önerileri gibi seyahatle ilgili konulara cevap vermektir. " +

                      "**DAVRANIŞ KURALLARI:** " +
                      "1. BİLGİYİ AŞAMA AŞAMA VER: Kullanıcıya asla tek seferde uzun listeler veya metin yığınları gönderme. " +
                      "2. İNTERAKTİF OL: Her cevabının sonunda kullanıcıya bir soru sorarak diyaloğu devam ettir. (Örn: 'Uçuş stratejilerinden bahsedeyim mi?', 'Bu vize türü size uygun mu?') " +
                      "3. SINIRLARINI KORU: Eğer kullanıcı matematik, genel kültür, yazılım veya seyahat dışı herhangi bir soru sorarsa, nazikçe şu kalıbı kullan: " +
                      "'Üzgünüm, ben sadece seyahat ve gezi konularında yardımcı olabilen bir asistanım. Size planladığınız yolculuklar hakkında nasıl yardımcı olabilirim?' " +
                      "4. KISA VE ÖZ OL: Her mesajın maksimum 2-3 kısa paragraftan oluşmalı. " +
                      "5. ULTRA KISA YANITLAR: Her mesajın mümkün olan en kısa, öz ve net şekilde olmalı. Mevcut uzun cevaplarını yarı yarıya kısaltarak gönder. " +
                      "6. MOBİL UYUMLU FORMAT: Paragraflarını 2 cümleyi geçmeyecek şekilde ayarla." +
                      "7. TEKRARLAYAN GİRİŞLERDEN KAÇIN: Her yanıtın başında 'Merhaba', 'Size yardımcı olmaktan memnuniyet duyarım' gibi kalıplaşmış ve kendini tekrar eden cümleler ASLA kullanma. " +
                      "8. Kullanıcının TÜRK VATANDAŞI olduğunu varsay. Vize durumlarını her zaman Türk pasaportu üzerinden değerlendir ve kullanıcıya vatandaşlık sorma. " +
                      "9. Vize bilgilerinde 'Bordo Pasaport' (Umuma Mahsus) kriterlerini baz al, istisnai durumlar varsa kısaca belirt. " +
                      "**İÇERİK STRATEJİSİ:** " +
                      "- Kullanıcı ucuz bilet sorarsa önce genel stratejiyi söyle, sonra detayları diğer mesajlara sakla. " +
                      "- Kullanıcı bir şehir sorarsa önce en popüler 2 yeri söyle ve 'Daha fazlasını listelememi ister misiniz?' diye sor.";
            #endregion

            var payload = new
            {
                contents = new[]
        {
            new
            {
                parts = new[]
                {
                    new { text = $"{systemPrompt}\n\nKullanıcı: {request.Message}" }
                }
            }
        }
            };

            try
            {
                var response = await _httpClient.PostAsJsonAsync(url, payload);
                var jsonString = await response.Content.ReadAsStringAsync();

                using var doc = System.Text.Json.JsonDocument.Parse(jsonString);
                var root = doc.RootElement;

                if (root.TryGetProperty("error", out var errorElement))
                {
                    return BadRequest(new { response = $"Gemini Hatası: {errorElement.GetProperty("message").GetString()}" });
                }

                var aiText = root.GetProperty("candidates")[0]
                                 .GetProperty("content")
                                 .GetProperty("parts")[0]
                                 .GetProperty("text")
                                 .GetString();

                return Ok(new { response = aiText });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { response = "Sistem hatası: " + ex.Message });
            }
        }
    }
}
