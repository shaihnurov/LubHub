Полный список API Endpoints для Frontend
REST API Endpoints
Authentication
1. GET /api/v1/auth/twitch/login
Описание: Возвращает URL для OAuth авторизации через Twitch
Headers: Нет
Request: Нет
Response:
{
  "url": "https://id.twitch.tv/oauth2/authorize?client_id=...&redirect_uri=...&response_type=code&scope=..."
}
Используется в: src/lib/auth.tsx → loginWithTwitch()
2. GET /api/v1/auth/twitch/callback?code={code}
Описание: Обменивает authorization code на JWT token
Headers: Нет
Query Parameters:
- code (string, required) - Authorization code от Twitch OAuth
Response:
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "displayName": "StreamerName"
}
Используется в: src/app/auth/callback/page.tsx
Raffles
3. GET /api/v1/raffles/my
Описание: Список раффлов текущего стримера (авторизованного пользователя)
Headers:
Authorization: Bearer {token}
Request: Нет
Response:
[
  {
    "id": 1,
    "streamerId": 123,
    "title": "Steam Key Giveaway",
    "status": "Active",
    "startedAt": "2026-06-20T10:00:00Z",
    "endedAt": null,
    "participantCount": 8742,
    "streamerName": "xQC",
    "winner": null
  }
]
Используется в: src/app/dashboard/page.tsx → useSWR("my-raffles", api.raffles.listMy)
4. GET /api/v1/raffles
Описание: Публичный список всех раффлов (для каталога и секции Giveaways)
Headers: Нет (публичный endpoint)
Query Parameters:
- status (string, optional) - Фильтр по статусу: Active, Pending, Finished, Drawn
- limit (number, optional) - Максимальное количество раффлов (для Giveaways нужно 6)
Response:
[
  {
    "id": 1,
    "streamerId": 123,
    "title": "Steam Key Giveaway",
    "status": "Active",
    "startedAt": "2026-06-20T10:00:00Z",
    "endedAt": null,
    "participantCount": 8742,
    "streamerName": "xQC",
    "winner": null
  }
]
Используется в:
- src/components/sections/Giveaways.tsx → useSWR("giveaways-top", api.raffles.listAll)
- src/app/raffles/page.tsx → useSWR("all-raffles", api.raffles.listAll)
5. GET /api/v1/raffles/{id}
Описание: Детальная информация о конкретном раффле
Headers: Нет (публичный endpoint)
Path Parameters:
- id (number, required) - ID раффла
Response:
{
  "id": 1,
  "streamerId": 123,
  "title": "Steam Key Giveaway",
  "status": "Active",
  "startedAt": "2026-06-20T10:00:00Z",
  "endedAt": null,
  "participantCount": 8742,
  "streamerName": "xQC",
  "winner": {
    "winnerId": 42,
    "twitchUserId": "123456",
    "displayName": "LuckyViewer"
  }
}
Используется в:
- src/app/raffles/[id]/page.tsx → useSWR(raffleId ? raffle-${raffleId} : null, () => api.raffles.getById(raffleId))
- src/app/dashboard/raffles/[id]/page.tsx → useSWR(raffleId ? raffle-${raffleId} : null, () => api.raffles.getById(raffleId))
6. GET /api/v1/raffles/{id}/participants
Описание: Список участников конкретного раффла
Headers:
Authorization: Bearer {token}
Path Parameters:
- id (number, required) - ID раффла
Response:
[
  {
    "id": 1,
    "raffleId": 1,
    "twitchUserId": "user123",
    "displayName": "ViewerName",
    "botScore": 0.05,
    "isSuspected": false
  }
]
Используется в: src/app/dashboard/raffles/[id]/page.tsx → useSWR(raffleId ? raffle-${raffleId}-participants : null, () => api.raffles.getParticipants(raffleId))
7. POST /api/v1/raffles
Описание: Создать новый раффл
Headers:
Authorization: Bearer {token}
Content-Type: application/json
Request Body:
{
  "title": "Steam Key Giveaway"
}
Response:
{
  "id": 1
}
Используется в: src/app/dashboard/page.tsx → api.raffles.create({ title: newTitle.trim() })
8. POST /api/v1/raffles/{id}/start
Описание: Запустить раффл (изменить статус с Pending на Active)
Headers:
Authorization: Bearer {token}
Path Parameters:
- id (number, required) - ID раффла
Request: Нет
Response: 204 No Content
Используется в:
- src/app/dashboard/page.tsx → api.raffles.start(raffle.id)
- src/app/dashboard/raffles/[id]/page.tsx → api.raffles.start(raffleId)
9. POST /api/v1/raffles/{id}/finish
Описание: Завершить раффл (изменить статус с Active на Finished)
Headers:
Authorization: Bearer {token}
Path Parameters:
- id (number, required) - ID раффла
Request: Нет
Response: 204 No Content
Используется в:
- src/app/dashboard/page.tsx → api.raffles.finish(raffle.id)
- src/app/dashboard/raffles/[id]/page.tsx → api.raffles.finish(raffleId)
10. POST /api/v1/raffles/{id}/draw
Описание: Разыграть победителя (изменить статус с Finished на Drawn)
Headers:
Authorization: Bearer {token}
Path Parameters:
- id (number, required) - ID раффла
Request: Нет
Response:
{
  "winnerId": 42,
  "twitchUserId": "123456",
  "displayName": "LuckyViewer"
}
Используется в:
- src/app/dashboard/page.tsx → api.raffles.draw(raffle.id)
- src/app/dashboard/raffles/[id]/page.tsx → api.raffles.draw(raffleId)
11. POST /api/v1/raffles/{id}/join
Описание: Присоединиться к рафлу как участник
Headers:
Authorization: Bearer {token}
Path Parameters:
- id (number, required) - ID раффла
Request: Нет
Response: 204 No Content
Используется в: src/app/raffles/[id]/page.tsx → api.raffles.join(raffleId)
Profile
12. GET /api/v1/profile
Описание: Профиль текущего авторизованного пользователя со статистикой
Headers:
Authorization: Bearer {token}
Request: Нет
Response:
{
  "twitchId": "12345678",
  "displayName": "TestStreamer",
  "email": "test@example.com",
  "rafflesCreated": 5,
  "rafflesParticipated": 12,
  "wins": 2,
  "createdAt": "2026-01-15T10:00:00Z"
}
Используется в:
- src/app/dashboard/profile/page.tsx → useSWR("profile", api.profile.get)
- src/app/dashboard/settings/page.tsx → useSWR("profile", api.profile.get)
SignalR Hub
Hub Endpoint: /hubs/raffle
Описание: Real-time hub для получения обновлений о раффлах
Аутентификация:
const connection = new signalR.HubConnectionBuilder()
  .withUrl("http://localhost:5217/hubs/raffle", {
    accessTokenFactory: () => token
  })
  .withAutomaticReconnect([0, 2000, 5000, 10000, 30000])
  .build();
Client → Server Methods
1. JoinRaffleGroup(raffleId: number)
Описание: Подписаться на обновления конкретного раффла
Parameters:
- raffleId (number, required) - ID раффла
Используется в: src/app/raffles/[id]/page.tsx → joinRaffleGroup(raffleId)
2. LeaveRaffleGroup(raffleId: number)
Описание: Отписаться от обновлений раффла
Parameters:
- raffleId (number, required) - ID раффла
Используется в: src/app/raffles/[id]/page.tsx → leaveRaffleGroup(raffleId) (в cleanup функции useEffect)
Server → Client Events
1. ParticipantCountUpdated(count: number)
Описание: Количество участников в раффле изменилось
Parameters:
- count (number) - Новое количество участников
Используется в: src/app/raffles/[id]/page.tsx → handleParticipantCountUpdated
2. WinnerDrawn(twitchUserId: string, displayName: string)
Описание: Победитель разыгран
Parameters:
- twitchUserId (string) - Twitch ID победителя
- displayName (string) - Отображаемое имя победителя
Используется в: src/app/raffles/[id]/page.tsx → handleWinnerDrawn
3. JoinConfirmed(raffleId: number)
Описание: Подтверждение успешного присоединения к раффлу
Parameters:
- raffleId (number) - ID раффла, к которому пользователь присоединился
Используется в: src/app/raffles/[id]/page.tsx → handleJoinConfirmed
Типы данных
Raffle
interface Raffle {
  id: number;
  streamerId: number;
  title: string;
  status: "Pending" | "Active" | "Finished" | "Drawn";
  startedAt: string | null;
  endedAt: string | null;
  participantCount: number;
  streamerName: string;
  winner?: WinnerResponse | null;
}
WinnerResponse
interface WinnerResponse {
  winnerId: number;
  twitchUserId: string;
  displayName: string;
}
Participant
interface Participant {
  id: number;
  raffleId: number;
  twitchUserId: string;
  displayName: string;
  botScore: number;
  isSuspected: boolean;
}
UserProfile
interface UserProfile {
  twitchId: string;
  displayName: string;
  email: string;
  rafflesCreated: number;
  rafflesParticipated: number;
  wins: number;
  createdAt: string;
}
AuthResponse
interface AuthResponse {
  token: string;
  displayName: string;
}
CreateRaffleRequest
interface CreateRaffleRequest {
  title: string;
}
ApiError
interface ApiError {
  type: string;
  title: string;
  status: number;
  detail?: string;
}
Итого
REST API Endpoints: 12
- Authentication: 2
- Raffles: 9
- Profile: 1
SignalR Hub: 1
- Client → Server methods: 2
- Server → Client events: 3
Всего: 12 REST endpoints + 1 SignalR hub с 5 методами/событиями