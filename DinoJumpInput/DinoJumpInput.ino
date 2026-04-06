#include "pitches.h"

const int _jumpPin = 2;
const int _duckPin = 3;
const int _ledPin = 11;
const int _buzzerPin = 12;

int _lastJumpButtonState = 0;
int _lastDuckButtonState = 0;

// melody stuff
int _deathMelody[] = {
  NOTE_DS3, NOTE_C3, NOTE_FS2
};

int _deathNoteDurations[] = {
  4, 4, 1
};

int _jumpSound = NOTE_B5;
//

void setup() {
  pinMode(_ledPin, OUTPUT);
  pinMode(_jumpPin, INPUT);
  pinMode(_duckPin, INPUT);
  pinMode(_buzzerPin, OUTPUT);

  Serial.begin(9600);
  Serial.setTimeout(5); // very important for performance!
  Serial.flush();
}

void checkDeath() {
  String state = Serial.readStringUntil('.');
  delay(10);

  if (state.equals("DEATH")) {    
    digitalWrite(_ledPin, HIGH);

    for (int i = 0; i < 3; i++) {
      int noteDuration = 1000 / _deathNoteDurations[i];
      tone(_buzzerPin, _deathMelody[i], noteDuration);

      int pauseBetweenNotes = noteDuration * 1.30;
      delay(pauseBetweenNotes);
      noTone(_buzzerPin);
    }
    
    digitalWrite(_ledPin, LOW);
  }
}

void handleJump() {
  int jumpButtonState = digitalRead(_jumpPin);

  if (jumpButtonState == HIGH && jumpButtonState != _lastJumpButtonState) {
    Serial.println("JUMP");
    Serial.flush();

    int noteDuration = 1000 / 16;
    tone(_buzzerPin, _jumpSound, noteDuration);

    int pause = noteDuration * 1.30;
    delay(pause);

    noTone(_buzzerPin);
  }
  
  _lastJumpButtonState = jumpButtonState;
}

void handleDuck() {
  int duckButtonState = digitalRead(_duckPin);

  if (duckButtonState != _lastDuckButtonState) {
    if (duckButtonState == HIGH)
      Serial.println("DUCK");
    else if (duckButtonState == LOW)
      Serial.println("NO_DUCK");

    Serial.flush();
  }

  _lastDuckButtonState = duckButtonState;
}

void loop() {

  handleJump();
  handleDuck();
  checkDeath();

}
