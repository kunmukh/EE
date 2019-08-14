#lang eopl
;KUNAL MUKHERJEE

;scanner
(define scanner-spec-let
  '((white-sp (whitespace) skip)
    (comment ("%" (arbno (not #\newline))) skip)
    (identifier (letter (arbno (or letter digit))) symbol)
    (number ((or "-" "+" digit)(arbno digit)) number)))

(define grammar-let
  '((E (number)
         num-Exp)
    (E ("+" "(" E "," E ")")
         plus-Exp)
    (E ("-" "(" E "," E ")")
         minus-Exp)
    (E ("*" "(" E "," E ")")
         mult-Exp)
    (E ("/" "(" E "," E ")")
         div-Exp)))   

(sllgen:make-define-datatypes scanner-spec-let grammar-let)

(define list-the-datatypes
  (lambda ()
    (sllgen:list-define-datatypes scanner-spec-let grammar-let)))

(define just-scan
  (sllgen:make-string-scanner scanner-spec-let grammar-let))

(define scan&parse
  (sllgen:make-string-parser scanner-spec-let grammar-let))

(define run
  (lambda (str)
    (value-of (scan&parse str))))

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
        "+(-(4,2),+(*(3,3),2))")

(define Morse_1
        "/(/(/(8,2),2),2)")
