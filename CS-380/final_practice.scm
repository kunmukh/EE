#lang eopl
;Kunal Mukherjee

(define-datatype Binary-Tree Binary-Tree?
  (empty-tree)
  (leaf)
  (branch (B1 Binary-Tree?)
          (B2 Binary-Tree?)))

(define height
  (lambda (B1-tree)
    (cases Binary-Tree B1-tree
      (empty-tree ()
                  -1)
      (leaf ()
                1)
      (branch (B1 B2)
                    (+ 1 (max (height B1) (height B2)))))))

(define num-leaves
  (lambda (B1-tree)
    (cases Binary-Tree B1-tree
      (empty-tree ()
                  0)
      (leaf ()
                1)
      (branch (B1 B2)
                    (+ (num-leaves B1) (num-leaves B2))))))

(define bt-parser
  (lambda (lst)
    (cond
      ((symbol? lst)
       (if (eqv? lst 'empty-tree)
           (empty-tree)
           (leaf)))
       ((pair? lst)
        (branch
         (bt-parser (car lst))
         (bt-parser (cadr lst))))
    (else report-error))))
           
(define report-error
  (lambda ()
    (eopl:error 'Exp "Error")))
           
