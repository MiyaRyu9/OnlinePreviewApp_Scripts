using System.Collections;
using System.Collections.Generic;
using Claudia;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class CreateStreamMessage : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        var anthropic = new Anthropic();

        UniTask.Void(async () =>
        {
            var messages = new Message[]
            {
            new() { Role = Roles.User, Content = "こんにちは、Claude" }
            };

            var messageStreamEvents = anthropic.Messages.CreateStreamAsync(new()
            {
                Model = Models.Claude3Haiku,
                MaxTokens = 1024,
                Messages = messages
            },
                cancellationToken: destroyCancellationToken);

            // IAsyncEnumerable は await foreach を使用して要素を取り出せる
            await foreach (var messageStreamEvent in messageStreamEvents)
            {
                // キャスト可能な型
                // https://github.com/Cysharp/Claudia/blob/main/src/Claudia/IMessageStreamEvent.cs

                switch (messageStreamEvent)
                {
                    case ContentBlockDelta content:
                        Debug.Log(content.Delta);
                        break;

                    default:
                        Debug.Log($"[{messageStreamEvent.TypeKind}]");
                        break;
                }
            }
        });
    }
}
