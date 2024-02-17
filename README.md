# Vocabulary Trainer 2

`Vocabulary Trainer 2` is an CLI program that helps you to learn German
vocabulary in more efficient manner by applying a simple spaced repetition algorithm.

## Project setup

Before trying to setup this project, make sure that you have a properly
configured Google Cloud project and a Google Sheets spreadsheet located
on your Google Drive folder.

### Obtaining `Credentials.json`

1. Navigate to: [https://console.cloud.google.com/](https://console.cloud.google.com/)
2. Click on `APIs & Services` button under `Quick Access`
3. Find a link to `Google Drive API` and click on it
4. Choose `Credentials` from the left hand side panel
5. Under `OAuth 2.0 Client IDs` click on `Download OAuth client` button
6. A file should be downloaded, rename it to `Credentials.json`

### Obtaining `SpreadsheetKey` value

1. Open previously created spreadsheet in your browser
2. Make sure that the URL matches the following pattern: `https://docs.google.com/spreadsheets/d/{SpreadsheetKey}/edit`
3. Save the value of `SpreadsheetKey` for later usage

### Setting up the project

1. Clone the repository
2. Build the project using Visual Studio
3. Go to the directory with generated executable file
4. Paste previously obtained `Credentials.json` file to that directory

The structure of that file should be identical to this:

```json
{
  "installed": {
    "client_id": "{client-id}",
    "project_id": "{project-id}",
    "auth_uri": "https://accounts.google.com/o/oauth2/auth",
    "token_uri": "https://oauth2.googleapis.com/token",
    "auth_provider_x509_cert_url": "https://www.googleapis.com/oauth2/v1/certs",
    "client_secret": "{client-secret}",
    "redirect_uris": ["http://localhost"]
  }
}
```

5. Create folders `Cache` and `Data` in that same directory
6. Make sure that the value of `SpreadsheetKey` in `Config.json` is identical to that obtained previously

## How does the spaced repetition algorithm works?

Every added word is converted into a set of flashcards. Every flashcard has a
cooldown and you won't be able to practice it until enough time has passed.

Initial cooldown is set to 12 hours (configurable in `Config.json`), because
it doesn't make much sense to practice something which you have already practiced
by adding it to the program.

After waiting for 12 hours you will be able to practice a flashcard.

If your answer is correct its cooldown will increase from 1 day to 2 days
up to a maximum of 32 days (configurable in `Config.json`), every time the
cooldown will be multiplied by 2.

On the other hand if your answer is incorrect its cooldown will decrease from
32 to 16, 8, 4 days and so on until the minimum of 1 day.
