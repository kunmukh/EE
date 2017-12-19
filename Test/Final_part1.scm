#lang eopl
;KUNAL MUKHERJEE
;16 FINAL

(define-datatype E E?
  (num-Exp (var number?))
  (plus-Exp (var1 E?)
            (var2 E?))
  (minus-Exp (var1 E?)
            (var2 E?))
  (mult-Exp (var1 E?)
            (var2 E?))
  (div-Exp (var1 E?)
            (var2 E?)))

(define value-of
  (lambda (input)
    (cases E input
      (num-Exp (var)
               var)
      (plus-Exp (var1 var2)
                (+ (value-of var1) (value-of var2)) )
      (minus-Exp (var1 var2)
                (- (value-of var1) (value-of var2)))
      (mult-Exp (var1 var2)
                (* (value-of var1) (value-of var2)))
      (div-Exp (var1 var2)
                (/ (value-of var1) (value-of var2))))))

(define Morse
  (plus-Exp
  (minus-Exp (num-Exp 4) (num-Exp 2))
  (plus-Exp (mult-Exp (num-Exp 3) (num-Exp 3)) (num-Exp 2))))

(define Morse_1
  (div-Exp (div-Exp (div-Exp (num-Exp 8) (num-Exp 2)) (num-Exp 2)) (num-Exp 2)))