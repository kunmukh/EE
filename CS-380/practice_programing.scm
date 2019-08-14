;Kunal Mukherjee
;cs380
;9/20/2017

#lang eopl
;1  
(define sum
  (lambda (lst)
    (if (null? lst)
        0
        (+ (car lst) (sum (cdr lst)) ))))
;(sumint '(1 2 3))

;2
(define length-list
  (lambda (lst1)
    (list-length (length lst1))))

(define length
  (lambda (lgth)
    (if (null? lgth)
        0
        (+ 1 (length (cdr lgth))))))

(define list-length
  (lambda (num)
    (if (eqv? num 0)
        '()
        (cons num
              (list-length (- num 1)) ))))
;2_new
(define revind
  (lambda (lst)
    (if (null? lst)
        '()
        (cons (length lst) (revind (cdr lst))))))

;3
(define-datatype stack stack?
  (empty-stack)
  (non-empty-stack (num symbol?)
                   (stck stack?)))

(define empty-stack?
  (lambda (stk)
    (cases stack stk
      (empty-stack ()
                   #t)
      (else
       #f))))
;(define e-stk (empty-stack))
;(empty-stack? e-stk)
;(define p-stk
;        (non-empty-stack 19 (non-empty-stack 18 (empty-stack))))

(define top
  (lambda (stk)
    (cases stack stk
      (empty-stack ()
                   report-stack-underflow)
      (non-empty-stack (num stck)
                       num))))
;(top p-stk)

(define pop
  (lambda (stk)
    (cases stack stk
      (empty-stack ()
                   report-stack-underflow)
      (non-empty-stack (num stck)
                       stck))))
;(define s-stk (pop stk))

(define push
  (lambda (val stk)
    (cases stack stk
      (empty-stack()
                  non-empty-stack val stk)
      (non-empty-stack (num stck)
                       (non-empty-stack val stk)))))
(define push_new
  (lambda (val stk)
    (non-empty-stack val stk)))


(define report-stack-underflow
  (lambda ()
    (eopl:error 'empty-stack "Stack underflow")))
               






