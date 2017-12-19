;Kunal Mukherjee
;cs 380
;10/16/2017
#lang eopl
;1a)
(define-datatype polynomial polynomial?
  (zero-poly)
  (multi-poly (n integer?)
              (e natural?)
              (poly polynomial?)))

(define natural?
  (lambda (n)
    (if (integer? n)
        (if (>= n 0)
            #t
            #f)
         #f)))
;1b)
(define construct-poly
  (lambda (lst)
    (if (null? (car lst))
        (zero-poly)
        (multi-poly (car (car lst)) (car (car (cdr lst))) (construct-poly (cons (cdr (car lst))  (list (cdr (car (cdr lst))))))))))

(define construct-polya
  (lambda (lst)
    (cond
      ((null? (car lst)) (zero-poly))
      (else
        (multi-poly (car (car lst)) (car (car (cdr lst))) (construct-polya (cons (cdr (car lst))  (list (cdr (car (cdr lst)))))))))))
;2
(define make-concrete
  (lambda (polya)
    (list (aux_1 polya) (aux_2 polya))))

(define aux_1
  (lambda (polya)
    (cases polynomial polya
      (zero-poly ()
                 '())
      (multi-poly (n e poly)
                  (cons n (aux_1 poly))))))

(define aux_2
  (lambda (polya)
    (cases polynomial polya
      (zero-poly ()
                 '())
      (multi-poly (n e poly)
                  (cons e (aux_2 poly))))))



;3
(define value-of
  (lambda (polya x)
    (cases polynomial polya
      (zero-poly ()
                 0)
      (multi-poly (n e poly)
                   (+ (* n (expt x e)) (value-of poly x))))))

;4

(define dx
  (lambda (polya)
    (list (aux1_1 polya) (aux1_2 polya))))

(define aux1_1
  (lambda (polya)
    (cases polynomial polya
      (zero-poly ()
                 (list 0))
      (multi-poly (n e poly)
                  (cons (* n e) (aux1_1 poly))))))

(define aux1_2
  (lambda (polya)
    (cases polynomial polya
      (zero-poly ()
                 (list 0))
      (multi-poly (n e poly)
                  (cons (- e 1) (aux1_2 poly))))))
     

                  
                 