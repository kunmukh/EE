//File: huffman.h
// HUffman class creates a huffman tree and allows the code
// to be represented using a binary tree with certain properties
//-----------------------------------------------------------------
// Class: CS 215                     Instructor: Dr. Deborah Hwang
// Assignment:    Project 7          Date assigned:  4/19/2017
// Programmer:    Kunal Mukherjee    Date completed: 4/26/2017

//The directives are included here
#include <string>
#include <fstream>

//boundary checking
#ifndef HUFFMAN_H_
#define HUFFMAN_H_

//TreeNode struct is used
struct TreeNode;

class HuffmanTree
{
 public:

  //Default Constructor
  HuffmanTree();

  //Load
  void Load (std::istream& codefile); //REC & PASSBK: codefile

  //Copy Constructor
  HuffmanTree(const HuffmanTree & source);  //REC & PASSBK: source class

  //Destructor
  ~HuffmanTree();  

  //MakeEmpty
  void MakeEmpty();

  //Assignment operator
  HuffmanTree & operator = (const HuffmanTree & source);//REC & PASSBK: source

  //Write
  void Write (std::ostream & out) const;  //REC & PASSBK: output stream

  //Encode
  void Encode (std::istream & messageFile, //REC & PASSBK: input stream
	       std::ostream & out) const;  //REC & PASSBK: output stream

  //Decode
  void Decode (std::istream & messageFile, //REC & PASSBK: input stream
	       std::ostream & out) const; //REC & PASSBK: output stream

 private:
  TreeNode* root; //pointer to the root of the tree

};

#endif //HUFFMAN_H_
  

