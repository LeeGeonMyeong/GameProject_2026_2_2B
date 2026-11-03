# 10강 DB & NODEJS & UNITY 연결

수업 자료: https://pineapple-cupcake-2ad.notion.site/10-DB-NODEJS-UNITY-40d8daa8a4ba80b5abf3e7e1f00da933

1. MySQL 서버를 실행하고 `gametest` DB를 준비합니다. DB 접속 정보는 `Node/.env`에서 설정합니다. 새 PC에서는 `.env.example`을 `.env`로 복사하고 본인 비밀번호를 입력합니다. `.env`는 Git에 포함되지 않습니다. 서버는 Node.js 22.9 이상에서 실행합니다.
2. 이 폴더에서 `node GameServer.js`를 실행하고 터미널을 켜둡니다.
3. Unity에서 `Assets/Scenes/DBConnectionScene.unity`를 열고 Play를 누릅니다.
4. Hierarchy의 `GameDataManager`를 선택하면 Inspector의 Player Quests에서 결과를 볼 수 있습니다. Console에도 조회 결과가 표시됩니다.

수업과 동일하게 4번 플레이어를 조회합니다. 결과가 0개면 `player_quests`와 `quests`에 해당 데이터가 있는지 확인합니다. 기존 DB 데이터는 자동으로 추가하거나 변경하지 않습니다.

`DungeonServer.js`는 이전 던전 실습용입니다. 두 서버가 모두 3000번 포트를 사용하므로, 이전 서버가 실행 중이면 해당 터미널에서 Ctrl+C로 중지한 뒤 `GameServer.js`를 실행합니다. 기존 던전 장면은 10강 실습 장면과 별도입니다.

패키지를 새로 설치해야 하는 PC에서는 `npm.cmd install`을 실행합니다.
