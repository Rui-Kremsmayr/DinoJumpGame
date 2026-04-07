const int _jumpPin = 2;
const int _duckPin = 3;
const int _ledPin = 11;

int _lastJumpButtonState = 0;
int _lastDuckButtonState = 0;

void setup() {
  pinMode(_jumpPin, INPUT);
  pinMode(_duckPin, INPUT);
  pinMode(_ledPin, OUTPUT);

  Serial.begin(9600);
  Serial.setTimeout(5);
  Serial.flush();
}

void jump() {
  int jumpButtonState = digitalRead(_jumpPin);

  if (jumpButtonState == HIGH && jumpButtonState != _lastJumpButtonState) {
    Serial.println("JUMP");
    Serial.flush();
  }

  _lastJumpButtonState = jumpButtonState;
}

void duck() {
  int duckButtonState = digitalRead(_duckPin);

  if (duckButtonState != _lastDuckButtonState) {
    if (duckButtonState == HIGH)
      Serial.println("DUCK");
    else
      Serial.println("NO_DUCK");

    Serial.flush();
  }

  _lastDuckButtonState = duckButtonState;
}


void loop() {
  
  jump();
  duck();

}
